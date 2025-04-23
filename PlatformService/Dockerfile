FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["PlatformService.sln", "./"]
COPY ["PlatformService/*.csproj", "PlatformService/"]

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build and publish
RUN dotnet publish -c Release -o /app/publish "PlatformService/PlatformService.csproj"

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
EXPOSE 1515  
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80;http://+:1515
ENTRYPOINT ["dotnet", "PlatformService.dll"]