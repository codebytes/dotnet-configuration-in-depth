# 21 - Azure App Configuration + Refresh

Uses Azure App Configuration with dynamic refresh registration.

## Prerequisites

1. Assign the identity you will run with (Azure CLI login, Visual Studio sign-in, or managed identity) the **App Configuration Data Reader** role on the target store.
2. Provide the App Configuration endpoint via configuration, for example with user secrets:

```powershell
dotnet user-secrets set "AzureAppConfiguration:Endpoint" "https://<store-name>.azconfig.io" --project AzureAppConfiguration.csproj
```

## Run

```powershell
dotnet run --project 21-azure-app-configuration
```

## Key Points

- RBAC + `DefaultAzureCredential` removes the need for connection strings; only the endpoint is required.
- `AddAzureAppConfiguration` registers the sentinel-driven refresh for `TestApp:Settings`.
- `UseAzureAppConfiguration()` middleware ensures each request uses the latest configuration snapshot.
- `TestApp:Settings` binds to the `Settings` POCO used throughout the MVC app.
