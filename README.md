# Overview
Tool for scraping backend data from frontend code. Meant to find most of the things to give an overview of what is there and what to look closer at.

Extracts the following data:
- Paths
- Urls
- Websockets
- Grapghql queries and mutations
- Hardcoded API keys

## Requirements
dotnet 6 https://dotnet.microsoft.com/en-us/download

## Installing from source
```powershell
dotnet pack -c Release .\src\0mg.HttpMap\0mg.HttpMap.csproj
dotnet tool install --global --add-source .nupkg 0mg.HttpMap
```

## Example
```
httpmap -u https://example.com/main.js
```

Output
```
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
    "test.js"
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
  Data Scraper

Usage:
  0mg.HttpMap [options]

Options:
  -u, --uri <uri> (REQUIRED)    Uri to scrape. (Url or file path)
  -ua, --useragent <useragent>  Useragent to use for requests [default: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:104.0) Gecko/20100101
                                Firefox/104.0]
  -p, --proxy <proxy>           Proxy url
  --version                     Show version information
  -?, -h, --help                Show help and usage information
```