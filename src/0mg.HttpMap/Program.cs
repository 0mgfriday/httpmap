using System.CommandLine;

const string banner = @".__     __    __                                
|  |___/  |__/  |_______   _____ _____  ______  
|  |  \   __\   __\____ \ /     \\__  \ \____ \ 
|   Y  \  |  |  | |  |_> >  Y Y  \/ __ \|  |_> >
|___|  /__|  |__| |   __/|__|_|  (____  /   __/ 
     \/           |__|         \/     \/|__|    
https://github.com/0mgfriday/httpmap
";

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
    description: "Write output as json to specified file")
{
    IsRequired = false,
};

var quietOption = new Option<bool>(
    new string[] { "--quiet", "-q" },
    description: "Only print output")
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
    quietOption,
};

rootCommand.Description = "Tool for scraping backend data from frontend code.";
rootCommand.SetHandler(
    async (string target, string userAgent, string? proxyUrl, IEnumerable<string> headers, string? outFile, bool queit) =>
    {
        if (!queit)
        {
            Console.WriteLine(banner);
            Console.WriteLine($"Target Url: {target}");
        }

        await _0mg.HttpMap.HttpMap.ScrapeAsync(target, userAgent, proxyUrl, headers, outFile);
    },
    targetOption,
    userAgentOption,
    proxyOption,
    headersOption,
    outfileOption,
    quietOption);

return await rootCommand.InvokeAsync(args);