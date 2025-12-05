#!/bin/sh
set -euo pipefail

location=${LOCATION:-eastus}
rg=${RESOURCE_GROUP:-rg-dotnet}
acName=${APPCONFIG_NAME:-cayers-dotnet-ac}

script_dir="$(cd "$(dirname "$0")" && pwd)"
project_path="${script_dir}/AppConfigAdvanced.csproj"

az group create --name "${rg}" --location "${location}" --only-show-errors >/dev/null

if az appconfig show --name "${acName}" --resource-group "${rg}" --only-show-errors >/dev/null 2>&1; then
	echo "Using existing App Configuration instance '${acName}'"
else
	az appconfig create --location "${location}" --name "${acName}" --resource-group "${rg}" --only-show-errors >/dev/null
fi

az appconfig kv set --name "${acName}" --key Demo:SimpleValue --value "Hello from Azure App Configuration" -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key Demo:Nested:Value --value "Nested works from Azure App Configuration" -y --only-show-errors -o none
az appconfig kv set --name "${acName}" --key Sentinel --value v1 -y --only-show-errors -o none

feature_name=BetaFeature
az appconfig feature set --name "${acName}" --feature "${feature_name}" -y --only-show-errors -o none
az appconfig feature disable --name "${acName}" --feature "${feature_name}" -y --only-show-errors -o none

endpoint="https://${acName}.azconfig.io"
dotnet user-secrets init --project "${project_path}" >/dev/null
dotnet user-secrets set --project "${project_path}" "AzureAppConfiguration:Endpoint" "${endpoint}" >/dev/null

echo "Provisioned Demo:* keys, sentinel, and feature flag. Endpoint stored as AzureAppConfiguration:Endpoint user secret."