# 16 - Configuration Precedence & Launch Settings

Demonstrates how .NET layers configuration sources and how value precedence works across:

1. appsettings.json
2. appsettings.{Environment}.json
3. launchSettings.json (Development only via environment variables)
4. User Secrets (not shown here but would slot after env specific JSON in Development)
5. Environment Variables
6. Command Line arguments

The sample prints individual keys, the ordered list of configuration providers, and the full `GetDebugView()`.

## Run

```pwsh
dotnet run --project 16-configuration-precedence
```

## Experiments

Override via environment variable (PowerShell):

```pwsh
$env:Precedence__Setting2='FromEnvVar'
dotnet run --project 16-configuration-precedence --no-build
Remove-Item Env:Precedence__Setting2
```

Override via command line (highest precedence):

```pwsh
dotnet run --project 16-configuration-precedence -- --Precedence:Setting2 FromCmd
```

Force Production environment (removes Development file & launchSettings influence):

```pwsh
dotnet run --project 16-configuration-precedence -- --environment Production
```

Observe that `Precedence:SettingFromLaunch` only has a value when using the default Development profile with `dotnet run`.

## Key Teaching Points

* Order of resolution: last provider wins for a key.
* launchSettings.json injects environment variables only for local `dotnet run` when the profile is used.
* Command line arguments use colon (`:`) for nesting; environment variables use double underscore (`__`).
* `GetDebugView()` is invaluable for diagnostics in development.
