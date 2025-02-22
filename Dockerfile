FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 5001
EXPOSE 5000
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookManagementAPI.sln", "./"]
COPY ["BookManagement.API/BookManagement.API.csproj", "BookManagement.API/"]
COPY ["BookManagement.Models/BookManagement.Models.csproj", "BookManagement.Models/"]
COPY ["BookManagement.DataAccess/BookManagement.DataAccess.csproj", "BookManagement.DataAccess/"]
RUN dotnet restore "BookManagementAPI.sln"
COPY . .
WORKDIR "/src/BookManagement.API"
RUN dotnet build "BookManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookManagement.API.dll"]