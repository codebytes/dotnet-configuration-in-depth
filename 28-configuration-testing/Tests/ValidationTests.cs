using ConfigurationTesting.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ConfigurationTesting.Tests;

public class ValidationTests
{
    [Fact]
    public void DatabaseSettings_ShouldValidate_WithValidData()
    {
        // Arrange
        var settings = new DatabaseSettings
        {
            ConnectionString = "Server=localhost;Database=Valid;",
            TimeoutSeconds = 60,
            EnableRetries = true,
            MaxRetries = 5
        };

        // Act
        var validationResults = ValidateModel(settings);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void DatabaseSettings_ShouldFailValidation_WithInvalidData()
    {
        // Arrange
        var settings = new DatabaseSettings
        {
            ConnectionString = "Short", // Less than 10 characters
            TimeoutSeconds = 0, // Less than minimum range
            MaxRetries = 15 // Greater than maximum range
        };

        // Act
        var validationResults = ValidateModel(settings);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(DatabaseSettings.ConnectionString)));
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(DatabaseSettings.TimeoutSeconds)));
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(DatabaseSettings.MaxRetries)));
    }

    [Fact]
    public void DatabaseSettings_ShouldFailValidation_WithRequiredFieldMissing()
    {
        // Arrange
        var settings = new DatabaseSettings
        {
            ConnectionString = string.Empty, // Required field is empty
            TimeoutSeconds = 30
        };

        // Act
        var validationResults = ValidateModel(settings);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains(nameof(DatabaseSettings.ConnectionString)));
    }

    [Fact]
    public void OptionsValidation_ShouldWork_WithDependencyInjection()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Short", // Invalid - too short
            ["Database:TimeoutSeconds"] = "5000", // Invalid - too high
            ["Database:MaxRetries"] = "15" // Invalid - too high
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(DatabaseSettings.SectionName)
            .ValidateDataAnnotations();

        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        var optionsAccessor = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
        var exception = Assert.Throws<OptionsValidationException>(() => optionsAccessor.Value);
        
        Assert.Contains("ConnectionString", exception.Message);
        Assert.Contains("TimeoutSeconds", exception.Message);
        Assert.Contains("MaxRetries", exception.Message);
    }

    [Fact]
    public void OptionsValidation_ShouldWork_WithCustomValidation()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Server=localhost;Database=TestDB;",
            ["Database:TimeoutSeconds"] = "30",
            ["Database:MaxRetries"] = "3"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(DatabaseSettings.SectionName)
            .ValidateDataAnnotations()
            .Validate(settings => settings.ConnectionString.Contains("Database="), 
                     "Connection string must contain 'Database=' parameter")
            .Validate(settings => settings.TimeoutSeconds <= 300, 
                     "Timeout cannot exceed 5 minutes");

        var serviceProvider = services.BuildServiceProvider();

        // Act - Should not throw because validation passes
        var optionsAccessor = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
        var settings = optionsAccessor.Value;

        // Assert
        Assert.NotNull(settings);
        Assert.Equal("Server=localhost;Database=TestDB;", settings.ConnectionString);
    }

    [Fact]
    public void OptionsValidation_ShouldFail_WithCustomValidation()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Server=localhost;", // Missing Database= parameter
            ["Database:TimeoutSeconds"] = "600" // Too high (> 300)
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(DatabaseSettings.SectionName)
            .ValidateDataAnnotations()
            .Validate(settings => settings.ConnectionString.Contains("Database="), 
                     "Connection string must contain 'Database=' parameter")
            .Validate(settings => settings.TimeoutSeconds <= 300, 
                     "Timeout cannot exceed 5 minutes");

        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        var optionsAccessor = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>();
        var exception = Assert.Throws<OptionsValidationException>(() => optionsAccessor.Value);
        
        Assert.Contains("Connection string must contain 'Database=' parameter", exception.Message);
        Assert.Contains("Timeout cannot exceed 5 minutes", exception.Message);
    }

    [Fact]
    public void ValidateOnStart_ShouldFailEarly_WithInvalidConfiguration()
    {
        // Arrange
        var configData = new Dictionary<string, string?>
        {
            ["Database:ConnectionString"] = "Bad", // Too short
            ["Database:TimeoutSeconds"] = "0" // Invalid range
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddOptions<DatabaseSettings>()
            .BindConfiguration(DatabaseSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Act & Assert - Should fail during service provider build
        var serviceProvider = services.BuildServiceProvider();
        var exception = Assert.Throws<OptionsValidationException>(() => serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        Assert.Contains("ConnectionString", exception.Message);
        Assert.Contains("TimeoutSeconds", exception.Message);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        return validationResults;
    }
}