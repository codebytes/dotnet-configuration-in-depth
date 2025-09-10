using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

// Demo 24: Environment-specific configuration behavior
// Illustrates layering: appsettings.json -> appsettings.{Environment}.json -> User Secrets (Development) -> Environment Variables -> Command Line
// Run with different DOTNET_ENVIRONMENT or ASPNETCORE_ENVIRONMENT values and optional --LogLevel=Warning override.

var builder = Host.CreateApplicationBuilder(args);

// Bind strongly-typed options to observe overrides
builder.Services.Configure<DemoOptions>(builder.Configuration.GetSection("Demo"));

builder.Services.AddHostedService<Reporter>();

var host = builder.Build();
await host.RunAsync();

public sealed class DemoOptions
{
    public string Message { get; set; } = "(unset)";
    public string? Source { get; set; }
    public string LogLevel { get; set; } = "Info"; // Allow overriding via env/command-line
}

public sealed class Reporter(IConfiguration config, IOptionsMonitor<DemoOptions> monitor, IHostEnvironment env, ILogger<Reporter> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Dump resolved values and their effective environment
        var opts = monitor.CurrentValue;
        logger.LogInformation("Environment Name: {Env}", env.EnvironmentName);
        logger.LogInformation("Demo:Message = {Message}", opts.Message);
        logger.LogInformation("Demo:Source = {Source}", opts.Source);
        logger.LogInformation("Demo:LogLevel = {LogLevel}", opts.LogLevel);

        // Show a couple of specific keys straight from IConfiguration for clarity
        logger.LogInformation("Raw Configuration Value Demo:Message = {Val}", config["Demo:Message"]);

        logger.LogInformation("Change values and re-run with different environments (Development/Staging/Production). For command-line override try: --Demo:LogLevel=Warning");
        return Task.CompletedTask;
    }
}
