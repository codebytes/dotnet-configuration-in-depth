using ConfigurationTesting.Models;
using Microsoft.Extensions.Configuration;

namespace ConfigurationTesting.Tests;

public class ConfigurationBindingTests
{
    [Fact]
    public void DatabaseSettings_ShouldBindFromConfiguration()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Server=localhost;Database=TestDB;Integrated Security=true;",
            ["Database:TimeoutSeconds"] = "60",
            ["Database:EnableRetries"] = "false",
            ["Database:MaxRetries"] = "5"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        var settings = new DatabaseSettings();
        configuration.GetSection(DatabaseSettings.SectionName).Bind(settings);

        // Assert
        Assert.Equal("Server=localhost;Database=TestDB;Integrated Security=true;", settings.ConnectionString);
        Assert.Equal(60, settings.TimeoutSeconds);
        Assert.False(settings.EnableRetries);
        Assert.Equal(5, settings.MaxRetries);
    }

    [Fact]
    public void DatabaseSettings_ShouldUseDefaultValues_WhenConfigurationIsMissing()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var settings = new DatabaseSettings();
        configuration.GetSection(DatabaseSettings.SectionName).Bind(settings);

        // Assert
        Assert.Equal(string.Empty, settings.ConnectionString);
        Assert.Equal(30, settings.TimeoutSeconds); // Default value
        Assert.True(settings.EnableRetries); // Default value
        Assert.Equal(3, settings.MaxRetries); // Default value
    }

    [Fact]
    public void DatabaseSettings_ShouldGetDirectly_UsingGenericGet()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Server=prod;Database=ProdDB;",
            ["Database:TimeoutSeconds"] = "120"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        var settings = configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>();

        // Assert
        Assert.NotNull(settings);
        Assert.Equal("Server=prod;Database=ProdDB;", settings.ConnectionString);
        Assert.Equal(120, settings.TimeoutSeconds);
    }

    [Fact]
    public void Configuration_ShouldHandleComplexHierarchy()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["ConnectionStrings:Primary:Database:ConnectionString"] = "Primary DB Connection",
            ["ConnectionStrings:Primary:Database:TimeoutSeconds"] = "90",
            ["ConnectionStrings:Secondary:Database:ConnectionString"] = "Secondary DB Connection",
            ["ConnectionStrings:Secondary:Database:TimeoutSeconds"] = "45"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        var primarySettings = configuration.GetSection("ConnectionStrings:Primary:Database").Get<DatabaseSettings>();
        var secondarySettings = configuration.GetSection("ConnectionStrings:Secondary:Database").Get<DatabaseSettings>();

        // Assert
        Assert.NotNull(primarySettings);
        Assert.NotNull(secondarySettings);
        Assert.Equal("Primary DB Connection", primarySettings.ConnectionString);
        Assert.Equal(90, primarySettings.TimeoutSeconds);
        Assert.Equal("Secondary DB Connection", secondarySettings.ConnectionString);
        Assert.Equal(45, secondarySettings.TimeoutSeconds);
    }
}