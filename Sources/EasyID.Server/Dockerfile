FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS with-node
RUN apt-get update
RUN curl -sL https://deb.nodesource.com/setup_22.x | bash
RUN apt-get -y install nodejs
RUN apt-get clean && rm -rf /var/lib/apt/lists/*

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EasyID.Server/EasyID.Server.csproj", "EasyID.Server/"]
COPY ["easyid.client/easyid.client.esproj", "easyid.client/"]
RUN dotnet restore "./EasyID.Server/EasyID.Server.csproj"
COPY . .
WORKDIR "/src/EasyID.Server"
RUN dotnet build "./EasyID.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EasyID.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyID.Server.dll"]