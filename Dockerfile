# --- Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ActividadApp.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# --- Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# libgdiplus requerido por ClosedXML para generar Excel
RUN apt-get update && apt-get install -y --no-install-recommends libgdiplus && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Carpeta para evidencias persistentes
RUN mkdir -p /app/wwwroot/evidencias

EXPOSE 8080

ENTRYPOINT ["dotnet", "ActividadApp.dll"]
