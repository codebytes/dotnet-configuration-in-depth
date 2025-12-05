## Overview

- Loads secrets from Azure Key Vault via DefaultAzureCredential.
- Replaces local secrets with centralized storage.

## Run

```powershell
dotnet run --project 11-azure-keyvault/AzureKeyvault.csproj
```

## Notes

- Provide a vault name and ensure the executing identity has Key Vault access (e.g., Key Vault Secrets User).
