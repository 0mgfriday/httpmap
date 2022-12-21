using System.Text.RegularExpressions;

namespace _0mg.HttpMap.Scraper.Parsers
{
    internal class JSParser
    {
        public HashSet<string> GetPaths(string input)
        {
            var results = new HashSet<string>();
            results.UnionWith(ExtractPathStrings(input, '"'));
            results.UnionWith(ExtractPathStrings(input, '\''));
            results.UnionWith(ExtractPathStrings(input, '`'));

            return results;
        }

        IEnumerable<string> ExtractPathStrings(string input, char delimeter)
        {
            int start = 0;
            while (true)
            {
                var i = input.IndexOf('/', start);
                if (i == -1)
                    break;
                start = i + 1;

                var strStart = input.LastIndexOf(delimeter, i - 1, 100);
                if (strStart != -1)
                {
                    var maxSteps = i + 100 < input.Length ? 100 : input.Length - i - 1;
                    var strEnd = input.IndexOf(delimeter, i, maxSteps);
                    if (strEnd != -1)
                    {
                        var str = input.Substring(strStart + 1, strEnd - strStart - 1);
                        if (IsPath(str))
                            yield return str;
                    }
                }
            }
        }

        bool IsPath(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            if (!Uri.IsWellFormedUriString(str, UriKind.Relative))
                return false;
            if (str[0] == '@')
                return false;

            var extensionsToIgnore = new string[] { "js", "png", "jpg", "jpeg", "svg", "mp3" };
            var lastDot = str.LastIndexOf('.');
            if (lastDot != -1 && lastDot != str.Length -1)
            {
                var extension = str.Substring(lastDot + 1, str.Length - lastDot - 1);
                if (extensionsToIgnore.Contains(extension))
                    return false;
            }

            return true;
        }

        public HashSet<string> GetUrls(string input)
        {
            var regex = new Regex(@"((?:https|http)://[\w._\-]{6,253}(?:/[\w._\-?&%/~:\[\]!$&'\(\)\*+,;=]*)?)", RegexOptions.Compiled);
            var matches = regex.Matches(input);

            return matches
                .Select(x => x.Groups[1])
                .Select(x => x.Value)
                .Where(x => !x.Contains("www.w3.org"))
                .ToHashSet();
        }

        public HashSet<string> GetGraphQL(string input)
        {
            //var regex = new Regex(@"['`""][\n\t ]*((?:query|mutation) [\w\(\)\{\}\n $:!.,]+)['`""]", RegexOptions.Compiled);
            var regex = new Regex(@"['`""][\n\t ]*((?:query|mutation) \w+)(?:\(| *\{)", RegexOptions.Compiled);
            var matches = regex.Matches(input);

            return matches
                .Select(x => x.Groups[1])
                .Select(x => x.Value)
                .ToHashSet();
        }

        public HashSet<string> GetWebSockets(string input)
        {
            var regex = new Regex(@"['`""](wss://[\w\.\/\-:]+)['`""]", RegexOptions.Compiled);
            var matches = regex.Matches(input);

            return matches
                .Select(x => x.Groups[1])
                .Select(x => x.Value)
                .ToHashSet();
        }
    }
}
