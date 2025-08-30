FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]

# ENV NUGET_FALLBACK_PACKAGES=""

RUN dotnet restore "./WebApi/WebApi.csproj" # cache restore layer
COPY . .
WORKDIR "/src/WebApi"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./WebApi.csproj" --configuration $BUILD_CONFIGURATION --output /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" --configuration $BUILD_CONFIGURATION --output /app/publish  /p:UseAppHost=false

FROM base AS final
ARG BUILD_CONFIGURATION=Release
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]