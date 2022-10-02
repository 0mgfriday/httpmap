using System.CommandLine;

var targetOption = new Option<string>(
    new string[] { "--uri", "-u" },
    description: "Uri to scrape. (Url or file path)")
{
    IsRequired = true,
};

var userAgentOption = new Option<string>(
    new string[] { "--useragent", "-ua" },
    description: "Useragent to use for requests")
{
    IsRequired = false,
};
userAgentOption.SetDefaultValue("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:104.0) Gecko/20100101 Firefox/104.0");

var proxyOption = new Option<string?>(
    new string[] { "--proxy", "-p" },
    description: "Proxy url")
{
    IsRequired = false,
};

var rootCommand = new RootCommand
{
    targetOption,
    userAgentOption,
    proxyOption
};

rootCommand.Description = "Tool for scraping backend data from frontend code.";
rootCommand.SetHandler(
    async (string target, string userAgent, string? proxyUrl) =>
    {
        await _0mg.HttpMap.HttpMap.ScrapeAsync(target, userAgent, proxyUrl);
    },
    targetOption,
    userAgentOption,
    proxyOption);

return await rootCommand.InvokeAsync(args);