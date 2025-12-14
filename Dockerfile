# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY FutureV.sln ./
COPY FutureV.Core/FutureV.Core.csproj FutureV.Core/
COPY FutureV.Data/FutureV.Data.csproj FutureV.Data/
COPY FutureV.App/FutureV.App.csproj FutureV.App/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR /src/FutureV.App
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FutureV.App.dll"]
