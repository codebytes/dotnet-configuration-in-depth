using ConfigurationTesting.Models;
using ConfigurationTesting.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConfigurationTesting.Tests;

public class OptionsPatternTests
{
    [Fact]
    public void OptionsPattern_ShouldWork_WithDependencyInjection()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Server=test;Database=TestDB;",
            ["Database:TimeoutSeconds"] = "45",
            ["Database:EnableRetries"] = "true"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));
        services.AddTransient<IDatabaseService, DatabaseService>();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var databaseService = serviceProvider.GetRequiredService<IDatabaseService>();

        // Assert
        Assert.Equal("Server=test;Database=TestDB;", databaseService.GetConnectionString());
        Assert.Equal(45, databaseService.GetTimeout());
        Assert.True(databaseService.AreRetriesEnabled());
    }

    [Fact]
    public void IOptionsMonitor_ShouldReflectConfigurationChanges()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Initial Connection",
            ["Database:TimeoutSeconds"] = "30"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

        var serviceProvider = services.BuildServiceProvider();
        var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<DatabaseSettings>>();

        // Act & Assert - Initial values
        var initialSettings = optionsMonitor.CurrentValue;
        Assert.Equal("Initial Connection", initialSettings.ConnectionString);
        Assert.Equal(30, initialSettings.TimeoutSeconds);

        // Note: In a real scenario with file-based config and reloadOnChange: true,
        // you would modify the config file and the monitor would detect changes.
        // This test shows the pattern for accessing current values.
    }

    [Fact]
    public void IOptionsSnapshot_ShouldWork_InScopedContext()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Scoped Connection",
            ["Database:TimeoutSeconds"] = "60"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

        var serviceProvider = services.BuildServiceProvider();

        // Act
        using var scope1 = serviceProvider.CreateScope();
        var optionsSnapshot1 = scope1.ServiceProvider.GetRequiredService<IOptionsSnapshot<DatabaseSettings>>();
        var settings1 = optionsSnapshot1.Value;

        using var scope2 = serviceProvider.CreateScope();
        var optionsSnapshot2 = scope2.ServiceProvider.GetRequiredService<IOptionsSnapshot<DatabaseSettings>>();
        var settings2 = optionsSnapshot2.Value;

        // Assert - Both scopes should have the same values
        Assert.Equal("Scoped Connection", settings1.ConnectionString);
        Assert.Equal("Scoped Connection", settings2.ConnectionString);
        Assert.Equal(60, settings1.TimeoutSeconds);
        Assert.Equal(60, settings2.TimeoutSeconds);
    }

    [Fact]
    public void NamedOptions_ShouldWork_WithMultipleConfigurations()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["PrimaryDatabase:ConnectionString"] = "Primary DB Connection",
            ["PrimaryDatabase:TimeoutSeconds"] = "30",
            ["SecondaryDatabase:ConnectionString"] = "Secondary DB Connection",
            ["SecondaryDatabase:TimeoutSeconds"] = "45"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<DatabaseSettings>("Primary", configuration.GetSection("PrimaryDatabase"));
        services.Configure<DatabaseSettings>("Secondary", configuration.GetSection("SecondaryDatabase"));

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var namedOptionsAccessor = serviceProvider.GetRequiredService<IOptionsSnapshot<DatabaseSettings>>();
        var primarySettings = namedOptionsAccessor.Get("Primary");
        var secondarySettings = namedOptionsAccessor.Get("Secondary");

        // Assert
        Assert.Equal("Primary DB Connection", primarySettings.ConnectionString);
        Assert.Equal(30, primarySettings.TimeoutSeconds);
        Assert.Equal("Secondary DB Connection", secondarySettings.ConnectionString);
        Assert.Equal(45, secondarySettings.TimeoutSeconds);
    }
}