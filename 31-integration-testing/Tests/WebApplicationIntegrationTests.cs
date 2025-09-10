using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using static Microsoft.AspNetCore.Hosting.WebHostDefaults;

namespace IntegrationTesting.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Find the actual project directory
        var projectDir = FindProjectDirectory();
        
        builder.UseEnvironment("Test");
        builder.UseContentRoot(projectDir);
    }
    
    private static string FindProjectDirectory()
    {
        // Start from current directory and look for .csproj file
        var currentDir = Directory.GetCurrentDirectory();
        
        // Look in current directory first
        if (Directory.GetFiles(currentDir, "*.csproj").Any())
            return currentDir;
            
        // Navigate up from bin/Debug/net9.0 to project root
        var projectDir = Directory.GetParent(currentDir)?.Parent?.Parent?.FullName;
        if (projectDir != null && Directory.GetFiles(projectDir, "*.csproj").Any())
            return projectDir;
            
        // Fallback to current directory
        return currentDir;
    }
}

public class WebApplicationIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public WebApplicationIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ApiSettings_ShouldBeAccessible_ThroughEndpoint()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/settings");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var settings = JsonSerializer.Deserialize<ApiSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(settings);
        Assert.NotEmpty(settings.Name);
        Assert.NotEmpty(settings.Version);
    }

    [Fact]
    public async Task ApiHealth_ShouldReturn_ConfiguredVersion()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/health");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        
        
        using var document = JsonDocument.Parse(json);
        var status = document.RootElement.GetProperty("status").GetString();
        var version = document.RootElement.GetProperty("version").GetString();

        Assert.Equal("Healthy", status);
        Assert.NotNull(version);
        Assert.NotEmpty(version);
    }

    [Fact]
    public async Task ApiData_ShouldReturn_ConfiguredData()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/data");

        // Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        
        using var document = JsonDocument.Parse(json);
        var apiName = document.RootElement.GetProperty("apiName").GetString();
        var version = document.RootElement.GetProperty("version").GetString();
        var allowedHosts = document.RootElement.GetProperty("allowedHosts");

        Assert.NotNull(apiName);
        Assert.NotEmpty(apiName);
        Assert.NotNull(version);
        Assert.NotEmpty(version);
        Assert.Equal(JsonValueKind.Array, allowedHosts.ValueKind);
    }

    [Fact]
    public void Configuration_ShouldBeLoadedCorrectly_InTestEnvironment()
    {
        // Arrange & Act
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        // Assert
        Assert.NotNull(configuration["Api:Name"]);
        Assert.NotNull(configuration["Api:Version"]);
        Assert.Equal("Test", configuration["Api:Environment"]); // Should come from appsettings.Test.json
    }

    [Fact]
    public void HostEnvironment_ShouldBeTest()
    {
        // Arrange & Act
        var environment = _factory.Services.GetRequiredService<IHostEnvironment>();

        // Assert
        Assert.Equal("Test", environment.EnvironmentName);
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        // Find the actual project directory
        var projectDir = FindProjectDirectory();
        
        builder.UseEnvironment("Test");
        builder.UseContentRoot(projectDir);
        
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Add test-specific configuration
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Api:Name"] = "Test API Override",
                ["Api:Version"] = "1.0.0-test",
                ["Api:TimeoutSeconds"] = "5", // Faster for tests
                ["Api:EnableLogging"] = "false" // Reduce test noise
            });
        });

        builder.ConfigureServices(services =>
        {
            // Override services for testing if needed
            // For example, replace real database with in-memory database
        });
    }
    
    private static string FindProjectDirectory()
    {
        // Start from current directory and look for .csproj file
        var currentDir = Directory.GetCurrentDirectory();
        
        // Look in current directory first
        if (Directory.GetFiles(currentDir, "*.csproj").Any())
            return currentDir;
            
        // Navigate up from bin/Debug/net9.0 to project root
        var projectDir = Directory.GetParent(currentDir)?.Parent?.Parent?.FullName;
        if (projectDir != null && Directory.GetFiles(projectDir, "*.csproj").Any())
            return projectDir;
            
        // Fallback to current directory
        return currentDir;
    }
}

public class CustomConfigurationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CustomConfigurationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CustomConfiguration_ShouldOverride_DefaultSettings()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/settings");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var settings = JsonSerializer.Deserialize<ApiSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        Assert.NotNull(settings);
        Assert.Equal("Test API Override", settings.Name);
        Assert.Equal("1.0.0-test", settings.Version);
        Assert.Equal(5, settings.TimeoutSeconds);
        Assert.False(settings.EnableLogging);
    }

    [Fact]
    public async Task ApiData_ShouldRun_FasterWithTestConfiguration()
    {
        // Arrange
        var client = _factory.CreateClient();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await client.GetAsync("/api/data");
        stopwatch.Stop();

        // Assert
        response.EnsureSuccessStatusCode();
        // With TimeoutSeconds=5 and 10x multiplier, should take ~50ms plus HTTP overhead, much faster than default (300ms)
        Assert.True(stopwatch.ElapsedMilliseconds < 200, 
            $"API call took {stopwatch.ElapsedMilliseconds}ms, expected less than 200ms with test configuration");
    }
}

// Simple model for JSON deserialization in tests
public class ApiSettings
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; }
    public bool EnableLogging { get; set; }
    public List<string> AllowedHosts { get; set; } = new();
}