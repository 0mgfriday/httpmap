FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/0mg.HttpMap/0mg.HttpMap.csproj", "src/0mg.HttpMap/"]
COPY ["src/0mg.HttpMap.Scraper/0mg.HttpMap.Scraper.csproj", "src/0mg.HttpMap.Scraper/"]
RUN dotnet restore "src/0mg.HttpMap/0mg.HttpMap.csproj"
COPY . .
WORKDIR "/src/src/0mg.HttpMap"
RUN dotnet build "0mg.HttpMap.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "0mg.HttpMap.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "0mg.HttpMap.dll"]