Param(
    [string]$Location = "eastus",
    [string]$ResourceGroup = "rg-dotnet",
    [string]$AppConfigName = "cayers-dotnet-ac",
    [string]$LogAnalyticsName = "law-dotnet",
    [string]$AppInsightsName = "appi-dotnet"
)

$ErrorActionPreference = 'Stop'

$projectPath = Join-Path $PSScriptRoot 'AzureAppConfiguration.csproj'
if (-not (Test-Path $projectPath))
{
    throw "Unable to locate AzureAppConfiguration.csproj at $projectPath"
}

# Ensure required extensions are installed without prompts
az config set extension.use_dynamic_install=yes_without_prompt 2>$null

Write-Host "Creating resource group '$ResourceGroup'..."
az group create --name $ResourceGroup --location $Location --only-show-errors | Out-Null

Write-Host "Creating Log Analytics workspace '$LogAnalyticsName'..."
$lawId = az monitor log-analytics workspace create --workspace-name $LogAnalyticsName --resource-group $ResourceGroup --location $Location --query id -o tsv --only-show-errors

Write-Host "Creating Application Insights '$AppInsightsName'..."
az monitor app-insights component create --app $AppInsightsName --resource-group $ResourceGroup --location $Location --workspace $lawId --only-show-errors | Out-Null
$appiConnectionString = az monitor app-insights component show --app $AppInsightsName --resource-group $ResourceGroup --query connectionString -o tsv --only-show-errors

Write-Host "Creating App Configuration '$AppConfigName'..."
az appconfig create --location $Location --name $AppConfigName --resource-group $ResourceGroup --only-show-errors | Out-Null

# Configure diagnostic settings
$appConfigId = az appconfig show --name $AppConfigName --resource-group $ResourceGroup --query id -o tsv --only-show-errors
Write-Host "Creating diagnostic settings for App Configuration..."
az monitor diagnostic-settings create `
    --name "$AppConfigName-diag" `
    --resource $appConfigId `
    --workspace $lawId `
    --logs '[{"category":"HttpRequest","enabled":true},{"category":"Audit","enabled":true}]' `
    --metrics '[{"category":"AllMetrics","enabled":true}]' `
    --only-show-errors | Out-Null

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
dotnet user-secrets set --project $projectPath 'ApplicationInsights:ConnectionString' $appiConnectionString | Out-Null

Write-Host "Setup complete. User secrets configured."
