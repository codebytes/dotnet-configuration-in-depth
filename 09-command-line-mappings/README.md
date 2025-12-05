## Overview

- Maps friendly switches to configuration keys with AddCommandLine(args, mappings).
- Still supports raw colon-delimited arguments when needed.

## Run

```powershell
dotnet run --project 09-command-line-mappings/CommandLineMappings.csproj -- --color=green
```

### Additional Examples

```bash
dotnet run --project 09-command-line-mappings -- --greeting:color=blue
dotnet run --project 09-command-line-mappings -- --color=blue
```
