## Overview

- Adds command-line configuration for highest-precedence overrides.
- Mixes environment variables and switches to highlight precedence order.

## Run

```powershell
dotnet run --project 05-command-line-args/CommandLineArgs.csproj -- --environment=prod --greeting=Hey
```

### Examples

```bash
dotnet run --project 05-command-line-args -- --environment=prod
environment=qa dotnet run --project 05-command-line-args
environment=qa dotnet run --project 05-command-line-args -- --environment=prod
greeting=hey dotnet run --project 05-command-line-args -- --environment=prod
```

## Notes

- Remember the -- separator so dotnet passes switches through to the app.
