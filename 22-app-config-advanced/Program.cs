using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.Azure.AppConfiguration.AspNetCore; // middleware extension

var builder = WebApplication.CreateBuilder(args);

var endpoint = builder.Configuration["AzureAppConfiguration:Endpoint"]
    ?? builder.Configuration["AzureAppConfiguration__Endpoint"];

if (string.IsNullOrWhiteSpace(endpoint))
{
    throw new InvalidOperationException("Configuration value 'AzureAppConfiguration:Endpoint' is required for RBAC authentication.");
}

builder.Configuration.AddAzureAppConfiguration(o =>
{
    o.Connect(new Uri(endpoint), new DefaultAzureCredential())
     .ConfigureRefresh(r => r.Register("Sentinel", refreshAll: true)
                             .SetRefreshInterval(TimeSpan.FromSeconds(5)))
     .UseFeatureFlags(ff => ff.SetRefreshInterval(TimeSpan.FromSeconds(5)));
});
builder.Services.AddAzureAppConfiguration(); // adds middleware support
var useAzureAppConfig = true;

builder.Services.AddFeatureManagement();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (useAzureAppConfig)
{
    app.UseAzureAppConfiguration();
}

app.MapGet("/config", (IConfiguration config) =>
{
    // Show a few sample keys (adjust names as you create them in App Configuration)
    var sample = new
    {
        SimpleValue = config["Demo:SimpleValue"],
        Nested__Value = config["Demo:Nested:Value"],
        Timestamp = DateTimeOffset.UtcNow
    };
    return Results.Ok(sample);
});

app.MapGet("/feature/{name}", async (string name, IFeatureManager fm) =>
{
    var enabled = await fm.IsEnabledAsync(name);
    return Results.Ok(new { Feature = name, Enabled = enabled, Checked = DateTimeOffset.UtcNow });
});

app.MapGet("/refresh", (IConfiguration config) =>
{
    // Cast to IConfigurationRoot to access GetDebugView
    if (config is IConfigurationRoot root)
    {
        return Results.Text(root.GetDebugView());
    }
    return Results.Text("Debug view unavailable");
});

app.MapGet("/", () => Results.Ok(new
{
    Message = "Advanced App Configuration & Feature Flags Demo",
    Endpoints = new[] { "/config", "/feature/{name}", "/refresh" },
    Instructions = "Configure AzureAppConfiguration:Endpoint for DefaultAzureCredential access. Create keys 'Demo:SimpleValue', 'Demo:Nested:Value', the 'Sentinel' key, and feature flags. Modify values then bump 'Sentinel' to force refresh earlier." }
));

app.Run();
