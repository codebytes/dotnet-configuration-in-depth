#!/bin/sh
set -euo pipefail

location=${LOCATION:-eastus}
rg=${RESOURCE_GROUP:-rg-dotnet}
acName=${APPCONFIG_NAME:-cayers-dotnet-ac}

script_dir="$(cd "$(dirname "$0")" && pwd)"
project_path="${script_dir}/AzureAppConfiguration.csproj"

az group create --name "${rg}" --location "${location}" --only-show-errors >/dev/null

if az appconfig show --name "${acName}" --resource-group "${rg}" --only-show-errors >/dev/null 2>&1; then
	echo "Using existing App Configuration instance '${acName}'"
else
	az appconfig create --location "${location}" --name "${acName}" --resource-group "${rg}" --only-show-errors >/dev/null
fi

az appconfig kv set --name "${acName}" --key TestApp:Settings:BackgroundColor --value White -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:FontColor --value Black -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:FontSize --value 40 -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:Message --value "Data from Azure App Configuration" -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key TestApp:Settings:Sentinel --value 1 -y --only-show-errors -o none

endpoint="https://${acName}.azconfig.io"
dotnet user-secrets init --project "${project_path}" >/dev/null
dotnet user-secrets set --project "${project_path}" "AzureAppConfiguration:Endpoint" "${endpoint}" >/dev/null

echo "Provisioned TestApp:Settings keys and stored AzureAppConfiguration:Endpoint via user secrets."