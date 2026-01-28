using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<DemoOptions>(builder.Configuration.GetSection("Demo"));
builder.Services.AddHostedService<Reporter>();

var host = builder.Build();
await host.RunAsync();

public class DemoOptions
{
    public string Message { get; set; } = "(unset)";
    public string? Source { get; set; }
}

public class Reporter(IOptionsMonitor<DemoOptions> monitor, IHostEnvironment env, ILogger<Reporter> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var opts = monitor.CurrentValue;
        logger.LogInformation("Environment: {Env}", env.EnvironmentName);
        logger.LogInformation("Demo:Message = {Message}", opts.Message);
        logger.LogInformation("Demo:Source = {Source}", opts.Source);
        return Task.CompletedTask;
    }
}
