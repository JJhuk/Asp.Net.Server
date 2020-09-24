# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Server/*.csproj ./Server/
COPY Domain/*.csproj ./Domain/
COPY Tests/*.csproj ./Tests/
RUN dotnet restore

# copy everything else and build app
COPY Server/ ./Server/
COPY Domain/ ./Domain/
WORKDIR /Server
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Server.dll"]