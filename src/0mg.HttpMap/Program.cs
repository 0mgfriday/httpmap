using System.CommandLine;

var targetOption = new Option<string>(
    new string[] { "--uri", "-u" },
    description: "Uri to scrape (Url or file path)")
{
    IsRequired = true,
};

var userAgentOption = new Option<string>(
    new string[] { "--useragent", "-ua" },
    description: "Useragent to use for requests")
{
    IsRequired = false,
};
userAgentOption.SetDefaultValue("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");

var proxyOption = new Option<string?>(
    new string[] { "--proxy", "-p" },
    description: "Proxy url")
{
    IsRequired = false,
};

var headersOption = new Option<IEnumerable<string>>(
    new string[] { "--header", "-H" },
    description: "Header for requests (Multiple Allowed)")
{
    IsRequired = false,
    AllowMultipleArgumentsPerToken = true,
};

var outfileOption = new Option<string?>(
    new string[] { "--outfile", "-o" },
    description: "Write output to specified file")
{
    IsRequired = false,
};

var rootCommand = new RootCommand
{
    targetOption,
    userAgentOption,
    proxyOption,
    headersOption,
    outfileOption,
};

rootCommand.Description = "Tool for scraping backend data from frontend code.";
rootCommand.SetHandler(
    async (string target, string userAgent, string? proxyUrl, IEnumerable<string> headers, string? outFile) =>
    {
        await _0mg.HttpMap.HttpMap.ScrapeAsync(target, userAgent, proxyUrl, headers, outFile);
    },
    targetOption,
    userAgentOption,
    proxyOption,
    headersOption,
    outfileOption);

return await rootCommand.InvokeAsync(args);