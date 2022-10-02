using System.Text.RegularExpressions;

namespace _0mg.HttpMap.Scraper.Parsers
{
    internal class JSParser
    {
        public HashSet<string> GetPaths(string input)
        {
            var regex = new Regex(@"['`""](\/(?:[\w\-_~!$&'\(\)%\{\}\+\\]+\/*){0,15})['`""]", RegexOptions.Compiled);
            var matches = regex.Matches(input);

            return matches
                .Select(x => x.Groups[1])
                .Select(x => x.Value)
                .Where(x => x.Length > 2)
                .ToHashSet();
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
