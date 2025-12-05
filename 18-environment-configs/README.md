## Overview

- Loads appsettings.{Environment}.json via DOTNET_ENVIRONMENT / ASPNETCORE_ENVIRONMENT.
- Shows environment variables and command-line overrides on top of those files.

## Run

```powershell
dotnet run --project 18-environment-configs/EnvironmentConfigs.csproj
```

## Notes

- Set DOTNET_ENVIRONMENT before running to swap between Development, Staging, and Production.
