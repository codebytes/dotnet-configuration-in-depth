# PowerShell script to set up Azure Key Vault
Param(
    [string]$Location = "eastus",
    [string]$ResourceGroup = "rg-dotnet",
    [string]$KeyVaultName = "cayersdotnetkv",
    [string]$LogAnalyticsName = "law-dotnet",
    [string]$AppInsightsName = "appi-dotnet"
)

$ErrorActionPreference = 'Stop'

# Ensure required extensions are installed without prompts
az config set extension.use_dynamic_install=yes_without_prompt 2>$null

Write-Host "Creating resource group '$ResourceGroup'..."
az group create --name $ResourceGroup --location $Location --only-show-errors | Out-Null

Write-Host "Creating Log Analytics workspace '$LogAnalyticsName'..."
$lawId = az monitor log-analytics workspace create --workspace-name $LogAnalyticsName --resource-group $ResourceGroup --location $Location --query id -o tsv --only-show-errors

Write-Host "Creating Application Insights '$AppInsightsName'..."
az monitor app-insights component create --app $AppInsightsName --resource-group $ResourceGroup --location $Location --workspace $lawId --only-show-errors | Out-Null
$appiConnectionString = az monitor app-insights component show --app $AppInsightsName --resource-group $ResourceGroup --query connectionString -o tsv --only-show-errors

Write-Host "Creating Key Vault '$KeyVaultName'..."
az keyvault create --location $Location --name $KeyVaultName --resource-group $ResourceGroup --only-show-errors | Out-Null

# Assign the Key Vault Administrator role to the current user
$userId = az ad signed-in-user show --query id --output tsv
$kvResourceId = az keyvault show --name $KeyVaultName --resource-group $ResourceGroup --query id --output tsv
az role assignment create --assignee $userId --role "Key Vault Administrator" --scope $kvResourceId --only-show-errors 2>$null | Out-Null

# Configure diagnostic settings
Write-Host "Creating diagnostic settings for Key Vault..."
az monitor diagnostic-settings create `
    --name "$KeyVaultName-diag" `
    --resource $kvResourceId `
    --workspace $lawId `
    --logs '[{"category":"AuditEvent","enabled":true},{"category":"AzurePolicyEvaluationDetails","enabled":true}]' `
    --metrics '[{"category":"AllMetrics","enabled":true}]' `
    --only-show-errors | Out-Null

az keyvault secret set --name Message --vault-name $KeyVaultName --value "Hello from KV" --only-show-errors | Out-Null

# Store Application Insights connection string in user secrets
$projectPath = Join-Path $PSScriptRoot 'AzureKeyvault.csproj'
if (Test-Path $projectPath)
{
    dotnet user-secrets init --project $projectPath 2>$null | Out-Null
    dotnet user-secrets set --project $projectPath 'ApplicationInsights:ConnectionString' $appiConnectionString | Out-Null
}

Write-Host "Setup complete. User secrets configured."
