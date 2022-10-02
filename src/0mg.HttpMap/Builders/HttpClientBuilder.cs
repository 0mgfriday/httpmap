using System.Net;

namespace _0mg.HttpMap.Builders
{
    internal class HttpClientBuilder
    {
        string? userAgent = null;
        string? proxyUrl = null;
        bool allowRedirects = true;

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

            return client;
        }
    }
}
