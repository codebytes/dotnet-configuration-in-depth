Param(
    [string]$Location = "eastus",
    [string]$ResourceGroup = "rg-dotnet",
    [string]$AppConfigName = "cayers-dotnet-ac"
)

$ErrorActionPreference = 'Stop'

$projectPath = Join-Path $PSScriptRoot 'AzureAppConfiguration.csproj'
if (-not (Test-Path $projectPath))
{
    throw "Unable to locate AzureAppConfiguration.csproj at $projectPath"
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
    'TestApp:Settings:BackgroundColor' = 'White'
    'TestApp:Settings:FontColor'       = 'Black'
    'TestApp:Settings:FontSize'        = '40'
    'TestApp:Settings:Message'         = 'Data from Azure App Configuration'
    'TestApp:Settings:Sentinel'        = '1'
}

foreach ($entry in $settings.GetEnumerator())
{
    az appconfig kv set --name $AppConfigName --key $entry.Key --value $entry.Value -y --only-show-errors --output none | Out-Null
}

# Create feature flag
az appconfig feature set --name $AppConfigName --feature Beta --description "Beta feature flag for testing" -y --only-show-errors --output none | Out-Null
az appconfig feature enable --name $AppConfigName --feature Beta -y --only-show-errors --output none | Out-Null
Write-Host "Created and enabled 'Beta' feature flag"

$endpoint = "https://$AppConfigName.azconfig.io"
dotnet user-secrets init --project $projectPath | Out-Null
dotnet user-secrets set --project $projectPath 'AzureAppConfiguration:Endpoint' $endpoint | Out-Null

Write-Host "Provisioned TestApp:Settings keys and stored AzureAppConfiguration:Endpoint via user secrets."
