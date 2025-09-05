FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Application/Application.csproj", "Application/"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]
RUN dotnet restore "./WebApi/WebApi.csproj"

COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "./WebApi.csproj" --output /app/build --no-restore

FROM build AS publish
RUN dotnet publish "./WebApi.csproj" --output /app/publish  /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]