# Overview
Tool for scraping backend data from a given file or url. For spa apps it will try to find the main or app js file to extract info from. Meant to find most of the things to give an overview of what is there and what to look closer at.


Extracts the following data:
- Paths
- Urls
- Websockets
- Grapghql queries and mutations
- Hardcoded API keys

## Requirements
dotnet 6 https://dotnet.microsoft.com/en-us/download

## Installation
```
dotnet tool install --global 0mg.HttpMap
```
You may need to add `$HOME/.dotnet/tools` to your path on Linux. You can do so by adding something like the following to your `.bashrc` or `.zshrc` file
```bash
export DOTNETTOOLS=$HOME/.dotnet/tools
export PATH=$DOTNETTOOLS:$PATH
```

### Installing from source
```powershell
dotnet pack -c Release .\src\0mg.HttpMap\0mg.HttpMap.csproj
dotnet tool install --global --add-source .nupkg 0mg.HttpMap
```

## Example
```
httpmap -u https://example.com
```

Output
```
.__     __    __
|  |___/  |__/  |_______   _____ _____  ______
|  |  \   __\   __\____ \ /     \\__  \ \____ \
|   Y  \  |  |  | |  |_> >  Y Y  \/ __ \|  |_> >
|___|  /__|  |__| |   __/|__|_|  (____  /   __/
     \/           |__|         \/     \/|__|
https://github.com/0mgfriday/httpmap

Target Url: https://example.com
{
  "Paths": [
    "/api/v1/users",
    "/graphql"
  ],
  "ExternalUrls": [
    "http://api.example.com"
  ],
  "WebSockets": [
    "wss://example"
  ],
  "FormActions": [],
  "JavaScriptFiles": [
    "app.js"
  ],
  "GraphQL": [
    "mutation AddUser",
    "query UserQuery"
  ],
  "Secrets": []
}
```

## Help
```
httpmap -h

Description:
  Tool for scraping backend data from frontend code.

Usage:
  0mg.HttpMap [options]

Options:
  -u, --uri <uri> (REQUIRED)    Uri to scrape (Url or file path)
  -ua, --useragent <useragent>  Useragent to use for requests [default: Mozilla/5.0 (Windows NT 10.0; Win64; x64;
                                rv:108.0) Gecko/20100101 Firefox/108.0]
  -p, --proxy <proxy>           Proxy url
  -H, --header <header>         Header for requests (Multiple Allowed)
  -o, --outfile <outfile>       Write output as json to specified file
  -q, --quiet                   Only print output
  --version                     Show version information
  -?, -h, --help                Show help and usage information
```