# 04 - Environment Variables

Adds `AddEnvironmentVariables()` showing how environment variables override JSON values.

## Run

```powershell
$env:environment = "qa"
dotnet run --project 04-environment-variables
```

### Bash

```bash
environment=qa dotnet run --project 04-environment-variables
```

## Key Points

- Provider order: JSON then Environment -> env vars win when keys collide.
- Cross-platform examples (PowerShell / bash) moved from code comments to README.

## Try

Set `greeting` or `environment` via environment variable and verify override.
