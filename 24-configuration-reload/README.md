## Overview

- Shows reloadOnChange: true combined with IOptionsMonitor<T>.
- Prints reload notifications when appsettings.json changes.

## Run

```powershell
dotnet run --project 24-configuration-reload/ConfigurationReload.csproj
```

## Notes

- Leave the app running and edit appsettings.json to trigger [Reload] logs.
