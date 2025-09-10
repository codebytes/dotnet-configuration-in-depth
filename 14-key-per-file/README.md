# 14 - Key Per File Provider

Reads individual config values from files in a directory using `AddKeyPerFile`.

## Directory

`keyPerFile/` contains one file per key. File name = key; file content = value.

## Run

```powershell
dotnet run --project 14-key-per-file
```

### Environment Override Examples (PowerShell)

```powershell
$Env:ENVIRONMENT="qa"; dotnet run --project 14-key-per-file
$Env:ENVIRONMENT="production"; dotnet run --project 14-key-per-file
$Env:ENVIRONMENT=$NULL; dotnet run --project 14-key-per-file
```

## Key Points

- Useful for Kubernetes secrets (mounted files).
- `optional: true` allows directory absence without failure.
- `GetDebugView()` demonstrates provider/value origins.
