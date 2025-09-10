# 19 - Environment-Specific Configuration

Demonstrates how .NET loads layered configuration based on the current environment.

Order of precedence in this sample (later wins if same key):
1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. User Secrets (when `Development` and user secrets added – not included by default here)
4. Environment Variables (e.g. `Demo__Message`)
5. Command-line (`--Demo:Message=...` or `--Demo:LogLevel=Warning`)

## Files
- `appsettings.json`: Base values.
- `appsettings.Development.json`: Development overrides.
- `appsettings.Staging.json`: Staging overrides.
- `appsettings.Production.json`: Production overrides (also bumps default LogLevel to Warning).

## Run Examples

```powershell
# Development (default if not set in many local setups)
$env:DOTNET_ENVIRONMENT='Development'
dotnet run --project 19-environment-configs

# Staging
$env:DOTNET_ENVIRONMENT='Staging'
dotnet run --project 19-environment-configs

# Production with additional command-line override
$env:DOTNET_ENVIRONMENT='Production'
dotnet run --project 19-environment-configs -- --Demo:LogLevel=Error

# Environment variable override of Message
$env:DOTNET_ENVIRONMENT='Staging'
$env:Demo__Message='From ENV VAR'
dotnet run --project 19-environment-configs
```

## Sample Output (abridged)
```
info: Reporter[0]
      Environment Name: Staging
info: Reporter[0]
      Demo:Message = Staging override message
info: Reporter[0]
      Demo:Source = appsettings.Staging.json
info: Reporter[0]
      Demo:LogLevel = Info
```
After setting `Demo__Message` env var:
```
info: Reporter[0]
      Demo:Message = From ENV VAR
info: Reporter[0]
      Demo:Source = appsettings.Staging.json
```

## Notes
- To illustrate user secrets, run `dotnet user-secrets init` inside the project directory, then `dotnet user-secrets set "Demo:Message" "Secret Dev Message"` and run with `DOTNET_ENVIRONMENT=Development`.
- `DOTNET_ENVIRONMENT` and `ASPNETCORE_ENVIRONMENT` are treated equivalently for generic vs web hosts; setting either works for this console host.
- Command-line `--Demo:LogLevel=Warning` overrides JSON + env variable values for `Demo:LogLevel`.
