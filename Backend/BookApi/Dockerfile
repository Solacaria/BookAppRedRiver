# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["BookApi.csproj", "."]
RUN dotnet restore "BookApi.csproj"

COPY . .
RUN dotnet publish "BookApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render provides PORT at runtime. 8080 is the local fallback.
EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet BookApi.dll --urls http://0.0.0.0:${PORT:-8080}"]
