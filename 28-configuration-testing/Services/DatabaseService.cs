using ConfigurationTesting.Models;
using Microsoft.Extensions.Options;

namespace ConfigurationTesting.Services;

public interface IDatabaseService
{
    string GetConnectionString();
    int GetTimeout();
    bool AreRetriesEnabled();
    Task<bool> TestConnectionAsync();
}

public class DatabaseService : IDatabaseService
{
    private readonly IOptions<DatabaseSettings> _options;

    public DatabaseService(IOptions<DatabaseSettings> options)
    {
        _options = options;
    }

    public string GetConnectionString() => _options.Value.ConnectionString;

    public int GetTimeout() => _options.Value.TimeoutSeconds;

    public bool AreRetriesEnabled() => _options.Value.EnableRetries;

    public async Task<bool> TestConnectionAsync()
    {
        // Simulate connection test
        await Task.Delay(100);
        return !string.IsNullOrWhiteSpace(_options.Value.ConnectionString);
    }
}