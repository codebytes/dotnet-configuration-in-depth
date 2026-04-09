using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
var appConfigEndpoint = builder.Configuration["AzureAppConfiguration:Endpoint"];

if (string.IsNullOrWhiteSpace(appConfigEndpoint))
{
    throw new InvalidOperationException("Configuration value 'AzureAppConfiguration:Endpoint' is required when using DefaultAzureCredential.");
}

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri(appConfigEndpoint), new DefaultAzureCredential())
        .UseFeatureFlags(featureFlagOptions =>
        {
            featureFlagOptions.SetRefreshInterval(TimeSpan.FromSeconds(5));
        })
        .ConfigureRefresh(refresh =>
                {
                    refresh.Register("TestApp:Settings:Sentinel", refreshAll: true)
                        .SetRefreshInterval(new TimeSpan(0, 0, 5));
                });
});

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAzureAppConfiguration();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
