# 21 - Azure App Configuration + Refresh

Uses Azure App Configuration with dynamic refresh registration.

## Prerequisites

Set the connection string (example PowerShell):

```powershell
$env:ConnectionStrings__AppConfig = "Endpoint=https://<store>.azconfig.io;Id=...;Secret=..."
```

## Run

```powershell
dotnet run --project 21-azure-app-configuration
```

## Key Points

- `AddAzureAppConfiguration` with sentinel key refresh pattern.
- `UseAzureAppConfiguration()` middleware for request pipeline integration.
- Binds `Settings` POCO from `TestApp:Settings`.
