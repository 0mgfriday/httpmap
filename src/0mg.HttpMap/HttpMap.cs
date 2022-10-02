using _0mg.HttpMap.Builders;
using Newtonsoft.Json;

namespace _0mg.HttpMap
{
    internal class HttpMap
    {
        public static async Task ScrapeAsync(string url, string userAgent, string? proxyUrl)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine("Invalid URI provided");
                System.Environment.Exit(1);
            }

            try
            {
                var uri = new Uri(url, UriKind.Absolute);

                var httpClient = BuildHttpClient(userAgent, proxyUrl);
                var scraper = new Scraper.Scraper(httpClient, uri);

                var data = await scraper.ScrapeAsync();
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);

                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to scrape data. {ex.Message}");
                Environment.Exit(1);
            }
        }

        static HttpClient BuildHttpClient(string userAgent, string? proxyUrl)
        {
            var builder = new HttpClientBuilder();
            builder.SetUseragent(userAgent);

            if (!string.IsNullOrEmpty(proxyUrl))
                builder.SetProxyUrl(proxyUrl);

            return builder.Build();
        }
    }
}
