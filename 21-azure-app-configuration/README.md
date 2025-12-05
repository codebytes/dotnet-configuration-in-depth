## Overview

- Connects to Azure App Configuration via DefaultAzureCredential and the store endpoint.
- Uses sentinel-driven refresh plus middleware per request.

## Run

```powershell
dotnet run --project 21-azure-app-configuration/AzureAppConfiguration.csproj
```

## Notes

- Grant App Configuration Data Reader and set AzureAppConfiguration:Endpoint via user secrets or environment variables before running.
