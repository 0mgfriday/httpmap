# Overview
Tool for extracting data such as URLs, GraphQL queries, sensitive info, etc. from a given file or url. For SPA apps it will try to find the main or app js file to extract info from.


Extracts the following data:
- Paths
- URLs
- Websockets
- Grapghql queries and mutations
- Hardcoded API keys

## Requirements
dotnet 6 https://dotnet.microsoft.com/en-us/download

## Installation
Download the nuget file from releases
```
dotnet tool install --global --add-source [DIRECTORY_CONTAINING_NUGET] 0mg.HttpMap --version 1.0.1
```

You may need to add `$HOME/.dotnet/tools` to your path on Linux. You can do so by adding something like the following to your `.bashrc` or `.zshrc` file
```bash
export DOTNETTOOLS=$HOME/.dotnet/tools
export PATH=$DOTNETTOOLS:$PATH
```

### From source
```powershell
dotnet pack -c Release .\src\0mg.HttpMap\0mg.HttpMap.csproj
dotnet tool install --global --add-source .nupkg 0mg.HttpMap
```

### Updating
```
dotnet tool update --global --add-source [DIRECTORY_CONTAINING_NUGET] 0mg.HttpMap --version 1.0.1
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
  "Secrets": [
    "API Key: 28b901e3041d5eddb024f7a581b78f76"
  ]
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