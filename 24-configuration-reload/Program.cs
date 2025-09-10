using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

// Top-level host bootstrap must come first
var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<MySettings>(builder.Configuration.GetRequiredSection("MySettings"));
builder.Services.AddHostedService<SettingsPrinter>();
using var host = builder.Build();
await host.RunAsync();

// Domain options
public sealed class MySettings
{
    public required string Greeting { get; set; }
    public int DelaySeconds { get; set; }
    public FeatureSettings Feature { get; set; } = new();
}

public sealed class FeatureSettings
{
    public bool Enabled { get; set; }
    public int Threshold { get; set; }
}

// Hosted service to display current settings periodically
public sealed class SettingsPrinter(IOptionsMonitor<MySettings> monitor, IHostEnvironment env, IConfiguration config) : BackgroundService
{
    private readonly IOptionsMonitor<MySettings> _monitor = monitor;
    private readonly IHostEnvironment _env = env;
    private readonly IConfiguration _config = config;
    private IDisposable? _reloadSubscription;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"Environment: {_env.EnvironmentName}");
        Console.WriteLine("Modify appsettings.json while the app runs to see reload.");

        _reloadSubscription = _monitor.OnChange(ms =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Reload] {DateTimeOffset.Now:T} -> Greeting='{ms.Greeting}', DelaySeconds={ms.DelaySeconds}, Feature.Enabled={ms.Feature.Enabled}, Threshold={ms.Feature.Threshold}");
            Console.ResetColor();
        });

        return RunLoop(stoppingToken);
    }

    private async Task RunLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var current = _monitor.CurrentValue;
            Console.WriteLine($"Current: Greeting='{current.Greeting}', DelaySeconds={current.DelaySeconds}, Feature.Enabled={current.Feature.Enabled}, Threshold={current.Feature.Threshold}");
            await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, current.DelaySeconds)), token);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _reloadSubscription?.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
