using System.Linq;
using System.Net;

namespace _0mg.HttpMap.Builders
{
    internal class HttpClientBuilder
    {
        string? userAgent = null;
        string? proxyUrl = null;
        bool allowRedirects = true;
        Dictionary<string, string> defaultHeaders = new Dictionary<string, string>();

        public void SetUseragent(string userAgent)
        {
            this.userAgent = userAgent;
        }

        public void SetProxyUrl(string proxyUrl)
        {
            if (!Uri.IsWellFormedUriString(proxyUrl, UriKind.Absolute))
                throw new ArgumentException("Invalid proxy url");

            this.proxyUrl = proxyUrl;
        }

        public void SetDefaultHeaders(IEnumerable<string> headers)
        {
            foreach (var header in headers)
            { 
                var parts = header.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    throw new ArgumentException($"Invalid header: {header}. Must be in format 'name: value'");

                defaultHeaders.Add(parts[0], parts[1]);
            }
        }

        public void SetAllowRedirects(bool allowRedirects)
        {
            this.allowRedirects = allowRedirects;
        }

        public HttpClient Build()
        {
            HttpClient client;
            if (!string.IsNullOrEmpty(proxyUrl))
            {
                var proxy = new WebProxy(new Uri(proxyUrl))
                { 
                    BypassProxyOnLocal = false, 
                };
                var clientHandler = new HttpClientHandler()
                {
                    AllowAutoRedirect = allowRedirects,
                    Proxy = proxy,
                    UseProxy = true
                };
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                client = new HttpClient(clientHandler);
            }
            else
            {
                var clientHandler = new HttpClientHandler()
                {
                    AllowAutoRedirect = allowRedirects
                };
                client = new HttpClient(clientHandler);
            }

            if (!string.IsNullOrEmpty(userAgent))
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            client.Timeout = TimeSpan.FromSeconds(10);

            foreach (var header in defaultHeaders)
            {
                client.DefaultRequestHeaders.Add(header.Key.Trim(), header.Value.Trim());
            }

            return client;
        }
    }
}
