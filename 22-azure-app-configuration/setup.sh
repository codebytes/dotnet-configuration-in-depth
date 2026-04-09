#!/bin/sh
set -euo pipefail

location=${LOCATION:-eastus}
rg=${RESOURCE_GROUP:-rg-dotnet}
acName=${APPCONFIG_NAME:-cayers-dotnet-ac}
lawName=${LOG_ANALYTICS_NAME:-law-dotnet}
appiName=${APP_INSIGHTS_NAME:-appi-dotnet}

script_dir="$(cd "$(dirname "$0")" && pwd)"
project_path="${script_dir}/AzureAppConfiguration.csproj"

# Ensure required extensions are installed without prompts
az config set extension.use_dynamic_install=yes_without_prompt 2>/dev/null || true

echo "Creating resource group '${rg}'..."
az group create --name "${rg}" --location "${location}" --only-show-errors >/dev/null

echo "Creating Log Analytics workspace '${lawName}'..."
lawId=$(az monitor log-analytics workspace create --workspace-name "${lawName}" --resource-group "${rg}" --location "${location}" --query id -o tsv --only-show-errors)

echo "Creating Application Insights '${appiName}'..."
az monitor app-insights component create --app "${appiName}" --resource-group "${rg}" --location "${location}" --workspace "${lawId}" --only-show-errors >/dev/null
appiConnectionString=$(az monitor app-insights component show --app "${appiName}" --resource-group "${rg}" --query connectionString -o tsv --only-show-errors)

echo "Creating App Configuration '${acName}'..."
az appconfig create --location "${location}" --name "${acName}" --resource-group "${rg}" --only-show-errors >/dev/null

# Configure diagnostic settings
appConfigId=$(az appconfig show --name "${acName}" --resource-group "${rg}" --query id -o tsv --only-show-errors)
echo "Creating diagnostic settings for App Configuration..."
az monitor diagnostic-settings create \
    --name "${acName}-diag" \
    --resource "${appConfigId}" \
    --workspace "${lawId}" \
    --logs '[{"category":"HttpRequest","enabled":true},{"category":"Audit","enabled":true}]' \
    --metrics '[{"category":"AllMetrics","enabled":true}]' \
    --only-show-errors >/dev/null

az appconfig kv set --name "${acName}" --key TestApp:Settings:BackgroundColor --value White -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:FontColor --value Black -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:FontSize --value 40 -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:Message --value "Data from Azure App Configuration" -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:Sentinel --value 1 -y --only-show-errors -o none

# Create feature flag
az appconfig feature set --name "${acName}" --feature Beta --description "Beta feature flag for testing" -y --only-show-errors -o none
az appconfig feature enable --name "${acName}" --feature Beta -y --only-show-errors -o none
echo "Created and enabled 'Beta' feature flag"

endpoint="https://${acName}.azconfig.io"
dotnet user-secrets init --project "${project_path}" >/dev/null
dotnet user-secrets set --project "${project_path}" "AzureAppConfiguration:Endpoint" "${endpoint}" >/dev/null
dotnet user-secrets set --project "${project_path}" "ApplicationInsights:ConnectionString" "${appiConnectionString}" >/dev/null

echo "Setup complete. User secrets configured."