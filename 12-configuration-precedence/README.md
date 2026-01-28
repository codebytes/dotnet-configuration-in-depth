## Overview

- Prints provider order to illustrate configuration precedence.
- Calls GetDebugView() to show where each value comes from.

## Run

```powershell
dotnet run --project 12-configuration-precedence/ConfigurationPrecedence.csproj
```

## Try These Examples

Try the following to observe precedence (PowerShell examples):

### 1. Baseline (appsettings + appsettings.Development + launchSettings.json overrides)

```powershell
dotnet run --project 12-configuration-precedence --no-build
```

### 2. Add environment variable (takes precedence over JSON files but below command line)

```powershell
$env:Precedence__Setting2='FromEnvVar'
dotnet run --project 12-configuration-precedence --no-build
```

### 3. Command line override of Setting2

```powershell
dotnet run --project 12-configuration-precedence -- --Precedence:Setting2 FromCmd
```

### 4. Command line provide a key only defined there

```powershell
dotnet run --project 12-configuration-precedence -- --Precedence:CommandLineOnly OnlyCmd
```

### 5. Observe launchSettings influence

The key `Precedence:SettingFromLaunch` is only set via launchSettings.json when using `dotnet run` (Development profile).

### 6. Use Production environment

Use `--environment Production` to see absence of appsettings.Development.json & launchSettings influence:

```powershell
dotnet run --project 12-configuration-precedence -- --environment Production
```

### Clean up

Clear environment variable after test:

```powershell
Remove-Item Env:Precedence__Setting2 -ErrorAction SilentlyContinue
```
