using AngleSharp.Html.Dom;

namespace _0mg.HttpMap.Scraper.Parsers
{
    internal class HtmlParser
    {
        readonly IHtmlDocument document;

        public HtmlParser(string html)
        { 
            var parser = new AngleSharp.Html.Parser.HtmlParser();
            document = parser.ParseDocument(html);
        }

        public IEnumerable<string> GetLinks()
        {
            foreach (var a in document.QuerySelectorAll("a"))
            { 
                var value = a.GetAttribute("href");
                if (!string.IsNullOrEmpty(value))
                    yield return value;
            }

            foreach (var a in document.QuerySelectorAll("meta"))
            {
                var value = a.GetAttribute("content");
                if (value != null && value.Contains("URL="))
                {
                    var url = value.Split("URL=", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    if (!string.IsNullOrEmpty(url))
                        yield return url;
                }
            }
        }

        public IEnumerable<string> GetJSFiles()
        {
            foreach (var a in document.QuerySelectorAll("script"))
            {
                var value = a.GetAttribute("src");
                if (!string.IsNullOrEmpty(value))
                    yield return value;
            }
        }

        public IEnumerable<string> GetInlineJS()
        {
            foreach (var a in document.QuerySelectorAll("script"))
            {
                var value = a.TextContent;
                if (!string.IsNullOrEmpty(value))
                    yield return value;
            }
        }

        public IEnumerable<string> GetFormActions()
        {
            foreach (var a in document.QuerySelectorAll("form"))
            {
                var value = a.GetAttribute("action");
                if (!string.IsNullOrEmpty(value))
                    yield return value;
            }
        }
    }
}
