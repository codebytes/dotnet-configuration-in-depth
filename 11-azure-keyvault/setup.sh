#!/bin/sh
set -euo pipefail

location=${LOCATION:-eastus}
rg=${RESOURCE_GROUP:-rg-dotnet}
kvName=${KEYVAULT_NAME:-cayersdotnetkv}
lawName=${LOG_ANALYTICS_NAME:-law-dotnet}
appiName=${APP_INSIGHTS_NAME:-appi-dotnet}

script_dir="$(cd "$(dirname "$0")" && pwd)"
project_path="${script_dir}/AzureKeyvault.csproj"

# Ensure required extensions are installed without prompts
az config set extension.use_dynamic_install=yes_without_prompt 2>/dev/null || true

echo "Creating resource group '${rg}'..."
az group create --name "${rg}" --location "${location}" --only-show-errors >/dev/null

echo "Creating Log Analytics workspace '${lawName}'..."
lawId=$(az monitor log-analytics workspace create --workspace-name "${lawName}" --resource-group "${rg}" --location "${location}" --query id -o tsv --only-show-errors)

echo "Creating Application Insights '${appiName}'..."
az monitor app-insights component create --app "${appiName}" --resource-group "${rg}" --location "${location}" --workspace "${lawId}" --only-show-errors >/dev/null
appiConnectionString=$(az monitor app-insights component show --app "${appiName}" --resource-group "${rg}" --query connectionString -o tsv --only-show-errors)

echo "Creating Key Vault '${kvName}'..."
az keyvault create --location "${location}" --name "${kvName}" --resource-group "${rg}" --only-show-errors >/dev/null

# Assign the Key Vault Administrator role to the current user
userId=$(az ad signed-in-user show --query id --output tsv)
kvResourceId=$(az keyvault show --name "${kvName}" --resource-group "${rg}" --query id --output tsv)
az role assignment create --assignee "${userId}" --role "Key Vault Administrator" --scope "${kvResourceId}" --only-show-errors 2>/dev/null || true

# Configure diagnostic settings
echo "Creating diagnostic settings for Key Vault..."
az monitor diagnostic-settings create \
    --name "${kvName}-diag" \
    --resource "${kvResourceId}" \
    --workspace "${lawId}" \
    --logs '[{"category":"AuditEvent","enabled":true},{"category":"AzurePolicyEvaluationDetails","enabled":true}]' \
    --metrics '[{"category":"AllMetrics","enabled":true}]' \
    --only-show-errors >/dev/null

az keyvault secret set --name Message --vault-name "${kvName}" --value "Hello from KV" --only-show-errors >/dev/null

# Store Application Insights connection string in user secrets
if [ -f "${project_path}" ]; then
    dotnet user-secrets init --project "${project_path}" >/dev/null 2>&1 || true
    dotnet user-secrets set --project "${project_path}" "ApplicationInsights:ConnectionString" "${appiConnectionString}" >/dev/null
fi

echo "Setup complete. User secrets configured."
