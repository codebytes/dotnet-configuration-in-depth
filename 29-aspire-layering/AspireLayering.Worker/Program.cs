using AspireLayering.SharedConfig;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<SharedMessageOptions>(builder.Configuration.GetSection("SharedMessage"));

builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();

await host.RunAsync();

public class WorkerService : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<WorkerService> _logger;

    public WorkerService(IConfiguration config, ILogger<WorkerService> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var interval = int.TryParse(_config["Worker:IntervalSeconds"], out var v) ? v : 5;
            var message = _config["SharedMessage:Message"]; // May be overridden by service/appsettings/env/parameter
            var apiKey = _config["Worker:ExternalApiKey"];
            _logger.LogInformation("Tick interval={Interval}s message={Message} apiKeyTail={Tail}", interval, message, apiKey?.Length > 4 ? apiKey[^4..] : "none");
            await Task.Delay(TimeSpan.FromSeconds(interval), stoppingToken);
        }
    }
}
