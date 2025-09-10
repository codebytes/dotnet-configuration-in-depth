using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(ApiSettings.SectionName));
builder.Services.AddSingleton<IApiService, ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapGet("/api/settings", (IOptions<ApiSettings> options) => options.Value);
app.MapGet("/api/health", (IApiService apiService) => new { Status = "Healthy", Version = apiService.GetVersion() });
app.MapGet("/api/data", (IApiService apiService) => apiService.GetDataAsync());

app.Run();

// Make the class public for testing
public partial class Program { }

public class ApiSettings
{
    public const string SectionName = "Api";

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Version { get; set; } = string.Empty;

    [Range(1, 3600)]
    public int TimeoutSeconds { get; set; } = 30;

    public bool EnableLogging { get; set; } = true;

    public List<string> AllowedHosts { get; set; } = new();
}

public interface IApiService
{
    string GetVersion();
    Task<object> GetDataAsync();
}

public class ApiService : IApiService
{
    private readonly ApiSettings _settings;

    public ApiService(IOptions<ApiSettings> options)
    {
        _settings = options.Value;
    }

    public string GetVersion() => _settings.Version;

    public async Task<object> GetDataAsync()
    {
        if (_settings.EnableLogging)
        {
            Console.WriteLine($"Fetching data for API: {_settings.Name}");
        }

        await Task.Delay(_settings.TimeoutSeconds * 10); // Simulate work

        return new
        {
            ApiName = _settings.Name,
            Version = _settings.Version,
            AllowedHosts = _settings.AllowedHosts,
            Timestamp = DateTime.UtcNow
        };
    }
}