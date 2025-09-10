# 21 - Advanced Azure App Configuration & Feature Flags

Shows how to:

* Connect to Azure App Configuration dynamically via connection string (env var or JSON)
* Use a refresh sentinel key to trigger full configuration reload
* Consume feature flags through `Microsoft.FeatureManagement`
* Inspect current configuration view & feature status

## Setup

1. Create (or reuse) an Azure App Configuration instance.
2. Add keys:
   * `Demo:SimpleValue` = `Hello from AppConfig`
   * `Demo:Nested:Value` = `Nested works`
   * `Sentinel` = `v1` (any value – change later to force global refresh)
3. Add one or more feature flags (e.g. `BetaFeature`).
4. Obtain the connection string from the portal and set an environment variable (PowerShell):

```pwsh
$env:AppConfig__ConnectionString = "<YourConnectionString>"
```

1. Run the project:

```pwsh
dotnet run --project 21-app-config-advanced
```

## Endpoints

| Endpoint | Description |
|----------|-------------|
| `/` | Basic metadata & instructions |
| `/config` | Returns example keys (Demo:*) |
| `/feature/{name}` | Returns feature flag state |
| `/refresh` | Returns `GetDebugView()` snapshot |

## Demonstrate Refresh

1. Hit `/config` – observe initial values.
2. In Azure App Configuration, change `Demo:SimpleValue` (value alone will update when cache expires ~5s).
3. To force immediate full refresh of everything (and especially feature flags/key changes not yet expired), update the `Sentinel` key (e.g. v2 -> v3). Within a few seconds the updated values appear.

## Feature Flags

Add a flag (e.g. `BetaFeature`) and toggle its state in the portal. Call:

```http
GET /feature/BetaFeature
```

and observe Enabled true/false flips without a restart.

## Teaching Points

* `ConfigureRefresh` with sentinel centralizes cache busting for related configuration keys & flags.
* Short cache expirations are fine for demos; production should use longer intervals to reduce load.
* `UseAzureAppConfiguration` middleware wires automatic refresh per request pipeline checkpoint.
* Feature flags integrate cleanly with DI via `IFeatureManager` / `[FeatureGate]` (not shown here) for conditional endpoints.

## Cleanup

Remove the environment variable when done:

```pwsh
Remove-Item Env:AppConfig__ConnectionString -ErrorAction SilentlyContinue
```
