using _0mg.HttpMap.Scraper.Model;

namespace _0mg.HttpMap.Scraper
{
    public class SpaScraper
    {
        readonly Scraper scraper;

        public SpaScraper(Scraper scraper)
        {
            this.scraper = scraper;
        }

        public async Task<PageData> ScrapeAsync(Uri url)
        {
            var pageData = await scraper.ScrapeAsync(url);
            var appJS = pageData.JavaScriptFiles.Where(f =>
                Uri.IsWellFormedUriString(f, UriKind.Relative) &&
                (f.ToLower().Contains("main") || f.ToLower().Contains("app")));

            foreach (var file in appJS)
            {
                var baseUrl = new Uri(url.ToString().Substring(0, url.ToString().LastIndexOf("/")));
                var fileUrl = new Uri(baseUrl, file);
                if (url.AbsolutePath != fileUrl.AbsolutePath)
                {
                    var jsData = await scraper.ScrapeAsync(fileUrl);
                    pageData.AddData(jsData);
                }
            }

            return pageData;
        }
    }
}
