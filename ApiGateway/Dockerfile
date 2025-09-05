FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateway/ApiGateway.csproj", "ApiGateway/"]

# ENV NUGET_FALLBACK_PACKAGES=""

RUN dotnet restore "./ApiGateway/ApiGateway.csproj" # cache restore layer
COPY . .
WORKDIR "/src/ApiGateway"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./ApiGateway.csproj" --configuration $BUILD_CONFIGURATION --output /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ApiGateway.csproj" --configuration $BUILD_CONFIGURATION --output /app/publish  /p:UseAppHost=false

FROM base AS final
ARG BUILD_CONFIGURATION=Release
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]