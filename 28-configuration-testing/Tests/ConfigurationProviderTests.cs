using Microsoft.Extensions.Configuration;

namespace ConfigurationTesting.Tests;

public class ConfigurationProviderTests
{
    [Fact]
    public void InMemoryConfiguration_ShouldWork_WithBasicKeyValuePairs()
    {
        // Arrange
        var keyValuePairs = new Dictionary<string, string?>
        {
            ["Setting1"] = "Value1",
            ["Setting2"] = "Value2",
            ["ComplexSection:SubSetting"] = "SubValue"
        };

        // Act
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(keyValuePairs)
            .Build();

        // Assert
        Assert.Equal("Value1", configuration["Setting1"]);
        Assert.Equal("Value2", configuration["Setting2"]);
        Assert.Equal("SubValue", configuration["ComplexSection:SubSetting"]);
    }

    [Fact]
    public void MultipleProviders_ShouldRespectPrecedenceOrder()
    {
        // Arrange
        var provider1Data = new Dictionary<string, string?>
        {
            ["Setting1"] = "FromProvider1",
            ["Setting2"] = "FromProvider1",
            ["UniqueToProvider1"] = "Value1"
        };

        var provider2Data = new Dictionary<string, string?>
        {
            ["Setting2"] = "FromProvider2", // This should override provider1
            ["Setting3"] = "FromProvider2",
            ["UniqueToProvider2"] = "Value2"
        };

        // Act
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(provider1Data) // Added first
            .AddInMemoryCollection(provider2Data) // Added second (higher precedence)
            .Build();

        // Assert
        Assert.Equal("FromProvider1", configuration["Setting1"]); // Only in provider1
        Assert.Equal("FromProvider2", configuration["Setting2"]); // Overridden by provider2
        Assert.Equal("FromProvider2", configuration["Setting3"]); // Only in provider2
        Assert.Equal("Value1", configuration["UniqueToProvider1"]);
        Assert.Equal("Value2", configuration["UniqueToProvider2"]);
    }

    [Fact]
    public void JsonConfiguration_ShouldWork_WithInMemoryJson()
    {
        // Arrange
        var json = """
        {
          "Database": {
            "ConnectionString": "Server=localhost;Database=TestDB;",
            "TimeoutSeconds": 45,
            "Features": {
              "EnableRetries": true,
              "LogQueries": false
            }
          },
          "Logging": {
            "Level": "Information"
          }
        }
        """;

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

        // Act
        var configuration = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        // Assert
        Assert.Equal("Server=localhost;Database=TestDB;", configuration["Database:ConnectionString"]);
        Assert.Equal("45", configuration["Database:TimeoutSeconds"]);
        Assert.Equal("True", configuration["Database:Features:EnableRetries"]);
        Assert.Equal("False", configuration["Database:Features:LogQueries"]);
        Assert.Equal("Information", configuration["Logging:Level"]);
    }

    [Fact]
    public void ConfigurationDebugView_ShouldShowProviderInformation()
    {
        // Arrange
        var memoryData = new Dictionary<string, string?>
        {
            ["Memory:Setting"] = "MemoryValue"
        };

        var json = """{"Json": {"Setting": "JsonValue"}}""";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(memoryData)
            .AddJsonStream(stream)
            .Build();

        // Act
        var debugView = configuration.GetDebugView();

        // Assert
        Assert.Contains("Setting=MemoryValue (MemoryConfigurationProvider)", debugView);
        Assert.Contains("Setting=JsonValue (JsonStreamConfigurationProvider)", debugView);
        Assert.Contains("Memory:", debugView);
        Assert.Contains("Json:", debugView);
    }

    [Fact]
    public void EnvironmentVariables_ShouldWork_WithPrefix()
    {
        // Arrange
        const string prefix = "MYAPP_";
        Environment.SetEnvironmentVariable($"{prefix}Setting1", "EnvValue1");
        Environment.SetEnvironmentVariable($"{prefix}Database__ConnectionString", "EnvConnectionString");

        try
        {
            // Act
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix)
                .Build();

            // Assert
            Assert.Equal("EnvValue1", configuration["Setting1"]);
            Assert.Equal("EnvConnectionString", configuration["Database:ConnectionString"]);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable($"{prefix}Setting1", null);
            Environment.SetEnvironmentVariable($"{prefix}Database__ConnectionString", null);
        }
    }

    [Theory]
    [InlineData("Database:ConnectionString", "Test Connection")]
    [InlineData("Nested:Level1:Level2:Value", "Deeply Nested")]
    [InlineData("SimpleKey", "SimpleValue")]
    public void ConfigurationAccess_ShouldWork_WithVariousKeyPatterns(string key, string expectedValue)
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Test Connection",
            ["Nested:Level1:Level2:Value"] = "Deeply Nested",
            ["SimpleKey"] = "SimpleValue"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        // Act & Assert
        Assert.Equal(expectedValue, configuration[key]);
    }

    [Fact]
    public void ConfigurationSection_ShouldWork_ForNestedAccess()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:Primary:ConnectionString"] = "Primary DB",
            ["Database:Primary:Timeout"] = "30",
            ["Database:Secondary:ConnectionString"] = "Secondary DB",
            ["Database:Secondary:Timeout"] = "45"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        // Act
        var databaseSection = configuration.GetSection("Database");
        var primarySection = databaseSection.GetSection("Primary");
        var secondarySection = databaseSection.GetSection("Secondary");

        // Assert
        Assert.Equal("Primary DB", primarySection["ConnectionString"]);
        Assert.Equal("30", primarySection["Timeout"]);
        Assert.Equal("Secondary DB", secondarySection["ConnectionString"]);
        Assert.Equal("45", secondarySection["Timeout"]);

        // Test section existence
        Assert.True(databaseSection.Exists());
        Assert.True(primarySection.Exists());
        Assert.False(configuration.GetSection("NonExistent").Exists());
    }
}