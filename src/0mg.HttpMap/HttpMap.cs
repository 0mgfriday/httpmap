using _0mg.HttpMap.Builders;
using _0mg.HttpMap.Scraper;
using Newtonsoft.Json;

namespace _0mg.HttpMap
{
    internal class HttpMap
    {
        public static async Task ScrapeAsync(string url, string userAgent, string? proxyUrl, IEnumerable<string> headers)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine("Invalid URI provided");
                Environment.Exit(1);
            }

            try
            {
                var uri = new Uri(url, UriKind.Absolute);

                var httpClient = BuildHttpClient(userAgent, proxyUrl, headers);
                var scraper = new Scraper.Scraper(httpClient);
                var spaScraper = new SpaScraper(scraper);

                var data = await spaScraper.ScrapeAsync(uri);
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to scrape data. {ex.Message}");
                Environment.Exit(1);
            }
        }

        static HttpClient BuildHttpClient(string userAgent, string? proxyUrl, IEnumerable<string> defaultHeaders)
        {
            var builder = new HttpClientBuilder();
            builder.SetUseragent(userAgent);

            if (!string.IsNullOrEmpty(proxyUrl))
                builder.SetProxyUrl(proxyUrl);

            builder.SetDefaultHeaders(defaultHeaders);

            return builder.Build();
        }
    }
}
