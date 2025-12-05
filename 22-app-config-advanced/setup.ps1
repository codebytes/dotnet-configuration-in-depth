Param(
    [string]$Location = "eastus",
    [string]$ResourceGroup = "rg-dotnet",
    [string]$AppConfigName = "cayers-dotnet-ac"
)

$ErrorActionPreference = 'Stop'

$projectPath = Join-Path $PSScriptRoot 'AppConfigAdvanced.csproj'
if (-not (Test-Path $projectPath))
{
    throw "Unable to locate AppConfigAdvanced.csproj at $projectPath"
}

az group create --name $ResourceGroup --location $Location --only-show-errors | Out-Null

az appconfig show --name $AppConfigName --resource-group $ResourceGroup --only-show-errors | Out-Null
if ($LASTEXITCODE -ne 0)
{
    az appconfig create --location $Location --name $AppConfigName --resource-group $ResourceGroup --only-show-errors | Out-Null
}
else
{
    Write-Host "Using existing App Configuration instance '$AppConfigName'"
}

$settings = @{
    'Demo:SimpleValue' = 'Hello from Azure App Configuration'
    'Demo:Nested:Value' = 'Nested works from Azure App Configuration'
    'Sentinel' = 'v1'
}

foreach ($entry in $settings.GetEnumerator())
{
    az appconfig kv set --name $AppConfigName --key $entry.Key --value $entry.Value -y --only-show-errors --output none | Out-Null
}

$featureName = 'BetaFeature'
az appconfig feature set --name $AppConfigName --feature $featureName -y --only-show-errors --output none | Out-Null
az appconfig feature disable --name $AppConfigName --feature $featureName -y --only-show-errors --output none | Out-Null

$endpoint = "https://$AppConfigName.azconfig.io"
dotnet user-secrets init --project $projectPath | Out-Null
dotnet user-secrets set --project $projectPath 'AzureAppConfiguration:Endpoint' $endpoint | Out-Null

Write-Host "Provisioned configuration values and feature flag. Endpoint stored in user secrets as AzureAppConfiguration:Endpoint."
