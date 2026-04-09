using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry();

// Get Key Vault name from configuration (set via environment variable or appsettings)
var keyVaultName = builder.Configuration["AzureKeyVault:Name"];
if (!string.IsNullOrWhiteSpace(keyVaultName))
{
    var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

    // Connect to Key Vault using DefaultAzureCredential
    // This supports managed identity in Azure and local development credentials
    builder.Configuration.AddAzureKeyVault(
        keyVaultUri,
        new DefaultAzureCredential(),
        new AzureKeyVaultConfigurationOptions
        {
            // Reload secrets every 5 minutes (optional - remove if not needed)
            ReloadInterval = TimeSpan.FromMinutes(5)
        });
}
else if (!builder.Environment.IsDevelopment())
{
    // Only require Key Vault in non-development environments
    throw new InvalidOperationException("Configuration value 'AzureKeyVault:Name' is required in non-development environments.");
}

var app = builder.Build();

// Access configuration values - Key Vault secrets override local config
var message = app.Configuration["Message"] ?? "No message configured";
var greeting = app.Configuration["greeting"] ?? "Hello";
var environment = app.Configuration["environment"] ?? "unknown";

Console.WriteLine($"Message: {message}");

var name = args.Length > 0 ? args[0] : "World";
Console.WriteLine($"{greeting}, {name}.");
Console.WriteLine($"Environment: {environment}");
