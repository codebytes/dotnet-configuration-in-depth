using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Demonstrates live reload of logging levels when appsettings.json changes.

var builder = Host.CreateApplicationBuilder(args);

// Host.CreateApplicationBuilder already adds appsettings.json & enables reloadOnChange=true by default.
// Nothing extra required here; we just rely on dynamic reload support.

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

builder.Services.AddHostedService<LoggingDemoService>();

await builder.Build().RunAsync();

internal sealed class LoggingDemoService(ILogger<LoggingDemoService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Logging reload demo started. Edit appsettings.json -> Logging:LogLevel:Default to change verbosity.");
        logger.LogInformation("Current Default Level will determine which messages appear.");
        var iteration = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            iteration++;
            logger.LogTrace("Trace   message {Iter}", iteration);
            logger.LogDebug("Debug   message {Iter}", iteration);
            logger.LogInformation("Info    message {Iter}", iteration);
            logger.LogWarning("Warning message {Iter}", iteration);
            logger.LogError("Error   message {Iter}", iteration);
            logger.LogCritical("Critical message {Iter}", iteration);
            await Task.Delay(2000, stoppingToken);
        }
    }
}
