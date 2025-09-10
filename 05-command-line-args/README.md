# 05 - Command Line Arguments

Adds `AddCommandLine(args)` to show highest-precedence overrides.

## Run

```powershell
dotnet run --project 05-command-line-args -- --environment=prod --greeting=Hey
```

## Precedence (Highest → Lowest)

1. Command line
2. Environment variables
3. JSON file

## Try

Combine env vars + command line to observe override order.

### Examples

```bash
dotnet run --project 05-command-line-args -- --environment=prod
environment=qa dotnet run --project 05-command-line-args
environment=qa dotnet run --project 05-command-line-args -- --environment=prod
greeting=hey dotnet run --project 05-command-line-args -- --environment=prod
```
