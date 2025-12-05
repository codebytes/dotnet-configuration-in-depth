## Overview

- Integration-tests an ASP.NET Core app with a custom WebApplicationFactory.
- Loads environment-specific appsettings for the Test environment.

## Run

```powershell
dotnet test 29-integration-testing/IntegrationTesting.csproj
```

## Notes

- Tests set the Test environment automatically, so no manual configuration is needed.
