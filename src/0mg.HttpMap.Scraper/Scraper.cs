using _0mg.HttpMap.Scraper.Parsers;
using _0mg.HttpMap.Scraper.Model;

namespace _0mg.HttpMap.Scraper
{
    public class Scraper
    {
        readonly HttpClient httpClient;

        public Scraper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PageData> ScrapeAsync(Uri uri)
        {
            string? content = null;
            string? contentType = null;

            if (!uri.IsAbsoluteUri)
                uri = new Uri(Path.Combine(Directory.GetCurrentDirectory(), uri.OriginalString));

            if (uri.IsFile)
            {
                if (!File.Exists(uri.OriginalString))
                    throw new Exception($"File {uri.OriginalString} does not exist");
                
                content = File.ReadAllText(uri.OriginalString);
            }
            else
            {
                var rsp = await httpClient.GetAsync(uri);

                if (rsp.IsSuccessStatusCode)
                {
                    content = await rsp.Content.ReadAsStringAsync();
                    contentType = rsp.Content.Headers.ContentType?.MediaType;
                }
            }

            if (!string.IsNullOrEmpty(content))
            {
                if (contentType == "application/javascript" || uri.PathAndQuery.EndsWith("js"))
                {
                    return GetDataFromJavaScript(content);
                }
                else
                    return GetDataFromHtml(content, uri.Host);
            }
            else
                return new PageData();
        }

        PageData GetDataFromJavaScript(string js)
        {
            var parser = new JSParser();
            var secretsParser = new SecretsParser();
            var pageData = new PageData();

            pageData.AddPaths(parser.GetPaths(js));
            pageData.AddExternalPaths(parser.GetUrls(js));
            pageData.AddWebSockets(parser.GetWebSockets(js));
            pageData.AddGraphQL(parser.GetGraphQL(js));
            pageData.AddSecrets(secretsParser.GetAPIKeys(js));
            
            return pageData;
        }

        PageData GetDataFromHtml(string html, string targetHost)
        {
            var parser = new HtmlParser(html);
            var pageData = new PageData();

            foreach (var l in parser.GetLinks())
            {
                if (Uri.IsWellFormedUriString(l, UriKind.Absolute))
                {
                    var uri = new Uri(l);
                    if (uri.Host == targetHost)
                        pageData.AddPath(uri.PathAndQuery);
                    else
                        pageData.AddExternalPath(l);
                }
                else
                {
                    pageData.AddPath(l);
                }
            }

            pageData.AddJavaScriptFiles(parser.GetJSFiles());
            pageData.AddFormActions(parser.GetFormActions());

            foreach (var js in parser.GetInlineJS())
            {
                var jsData = GetDataFromJavaScript(js);
                pageData.AddData(jsData);
            }

            return pageData;
        }
    }
}
