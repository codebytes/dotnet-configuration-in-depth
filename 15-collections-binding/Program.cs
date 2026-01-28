using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables(prefix: "COLLECTIONS_");

// Bind complex collection types
builder.Services.Configure<InventoryOptions>(builder.Configuration.GetSection("Inventory"));

// Demonstrate required member & collection binding
builder.Services.AddOptions<RequiredSettings>()
    .Bind(builder.Configuration.GetSection("RequiredSettings"))
    .ValidateDataAnnotations() // enforce [Required], [MinLength]
    .ValidateOnStart(); // fail fast if missing

builder.Services.AddHostedService<DemoService>();

var host = builder.Build();

try
{
    await host.StartAsync();
    await host.WaitForShutdownAsync();
}
catch (OptionsValidationException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Validation failed for RequiredSettings:");
    foreach (var failure in ex.Failures)
    {
        Console.WriteLine(" - " + failure);
    }
    Console.ResetColor();
}

// POCOs
public sealed class InventoryOptions
{
    public List<Warehouse> Warehouses { get; init; } = new();
}

public sealed class Warehouse
{
    public required string Name { get; init; } // C# required member
    public int Capacity { get; init; }
    public List<string> Tags { get; init; } = new();
    // Dictionary key -> bin code, value -> bin contents
    public Dictionary<string, Bin> Bins { get; init; } = new();
}

public sealed class Bin
{
    public string? Sku { get; init; }
    public int Quantity { get; init; }
}


public sealed class RequiredSettings
{
    [Required]
    [Url]
    public required string Endpoint { get; init; }

    [MinLength(1)]
    public List<string> ApiKeys { get; init; } = new();

    public Dictionary<string, bool> FeatureFlags { get; init; } = new();
}

public sealed class DemoService : BackgroundService
{
    private readonly IOptions<InventoryOptions> _inventory;
    private readonly IOptions<RequiredSettings> _required;

    public DemoService(IOptions<InventoryOptions> inventory, IOptions<RequiredSettings> required)
    {
        _inventory = inventory;
        _required = required;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("=== Collections Binding Demo ===");
        DumpInventory();
        Console.WriteLine();
        Console.WriteLine("=== Required Settings (fail-fast validated) ===");
        DumpRequired();
        Console.WriteLine();
        Console.WriteLine("You can experiment by removing values from appsettings.json and re-running to see validation failures.");
        return Task.CompletedTask;
    }

    private void DumpInventory()
    {
        foreach (var w in _inventory.Value.Warehouses)
        {
            Console.WriteLine($"Warehouse: {w.Name} (Capacity: {w.Capacity}) Tags=[{string.Join(',', w.Tags)}]");
            foreach (var kvp in w.Bins)
            {
                Console.WriteLine($"  Bin {kvp.Key}: Sku={kvp.Value.Sku} Qty={kvp.Value.Quantity}");
            }
        }
    }

    private void DumpRequired()
    {
        Console.WriteLine($"Endpoint: {_required.Value.Endpoint}");
        Console.WriteLine("API Keys: " + string.Join(',', _required.Value.ApiKeys));
        foreach (var flag in _required.Value.FeatureFlags)
        {
            Console.WriteLine($"Flag {flag.Key} = {flag.Value}");
        }
    }
}
