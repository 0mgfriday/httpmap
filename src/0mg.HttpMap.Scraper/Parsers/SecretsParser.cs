using System.Text.RegularExpressions;

namespace _0mg.HttpMap.Scraper.Parsers
{
    internal class SecretsParser
    {
        public HashSet<string> GetAPIKeys(string input)
        {
            var regex = new Regex(@"(?i)(?:api_key|api-key|apikey)(?:.{0,20})?['|""]([0-9a-zA-Z\-]{32,45})['|""]", RegexOptions.Compiled);
            var matches = regex.Matches(input);

            return matches
                .Select(x => x.Groups[1])
                .Select(x => x.Value)
                .ToHashSet();
        }
    }
}
