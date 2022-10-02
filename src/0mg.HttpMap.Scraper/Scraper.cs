using _0mg.HttpMap.Scraper.Parsers;
using _0mg.HttpMap.Scraper.Model;

namespace _0mg.HttpMap.Scraper
{
    public class Scraper
    {
        readonly HttpClient httpClient;
        readonly Uri baseUrl;

        readonly PageData pageData =  new();

        public Scraper(HttpClient httpClient, Uri baseUrl)
        {
            this.httpClient = httpClient;
            this.baseUrl = baseUrl;
        }

        public async Task<PageData> ScrapeAsync()
        {
            await ScrapeAsync(baseUrl);
            return pageData;
        }

        async Task ScrapeAsync(Uri url)
        {
            string? content = null;
            string? contentType = null;

            if (url.IsFile)
            {
                content = File.ReadAllText(url.AbsolutePath);
            }
            else
            {
                var rsp = await httpClient.GetAsync(url);

                if (rsp.IsSuccessStatusCode)
                {
                    content = await rsp.Content.ReadAsStringAsync();
                    contentType = rsp.Content.Headers.ContentType?.MediaType;
                }
            }

            if (!string.IsNullOrEmpty(content))
            {
                if (contentType == "application/javascript" || url.LocalPath.EndsWith("js"))
                {
                    GetDataFromJavaScript(content);
                }
                else
                    GetDataFromHtml(content);
            }
        }

        public void GetDataFromJavaScript(string js)
        {
            var parser = new JSParser();
            var secretsParser = new SecretsParser();

            pageData.AddPaths(parser.GetPaths(js));
            pageData.AddExternalPaths(parser.GetUrls(js));
            pageData.AddWebSockets(parser.GetWebSockets(js));
            pageData.AddGraphQL(parser.GetGraphQL(js));
            pageData.AddSecrets(secretsParser.GetAPIKeys(js));
        }

        public void GetDataFromHtml(string html)
        {
            var parser = new HtmlParser(html);
            foreach (var l in parser.GetLinks())
            {
                if (Uri.IsWellFormedUriString(l, UriKind.Absolute))
                {
                    var uri = new Uri(l);
                    if (uri.Host == baseUrl.Host)
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
                GetDataFromJavaScript(js);
            }
        }
    }
}
