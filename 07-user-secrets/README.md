## Overview

- Loads secrets from the user secrets store during Development.
- Keeps API keys out of source control.

## Run

```powershell
dotnet run --project 07-user-secrets/UserSecrets.csproj
```

## Notes

- Initialize secrets via 'dotnet user-secrets init --project 07-user-secrets' and set the expected keys before running.
