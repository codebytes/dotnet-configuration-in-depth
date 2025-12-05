using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

var effectiveEnvironment = ResolveEnvironment();

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    EnvironmentName = effectiveEnvironment
});

// Surface the resolved environment name via configuration without mutating process variables
builder.Configuration.AddInMemoryCollection(new[]
{
    new KeyValuePair<string, string?>("DOTNET_ENVIRONMENT", effectiveEnvironment)
});


// Bind strongly-typed options to observe overrides
builder.Services.Configure<DemoOptions>(builder.Configuration.GetSection("Demo"));

builder.Services.AddHostedService<Reporter>();

var host = builder.Build();
await host.RunAsync();

static string ResolveEnvironment() =>
    Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
    ?? Environments.Production;

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
