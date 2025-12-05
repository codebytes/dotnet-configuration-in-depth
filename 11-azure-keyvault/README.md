# 15 - Azure Key Vault

Demonstrates using `AddAzureKeyVault` to pull secrets from Azure Key Vault.

## Prerequisites

- Logged into Azure CLI / identity with access.
- Set `KeyVaultName` in environment or JSON.

## Run

```powershell
dotnet run --project 15-azure-keyvault -- KeyVaultName=<your-vault-name>
```

> Secrets in Key Vault appear as configuration keys; colon hierarchy often uses `--` or `--` to separate in naming (snake-case or double-dash patterns when naming inside vault).

## Key Points

- Uses `DefaultAzureCredential` (supports dev environments + managed identity).
- Great for central secret management; avoids user secrets in shared environments.
