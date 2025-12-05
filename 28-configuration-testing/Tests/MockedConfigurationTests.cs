using ConfigurationTesting.Models;
using ConfigurationTesting.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace ConfigurationTesting.Tests;

public class MockedConfigurationTests
{
    [Fact]
    public void DatabaseService_ShouldWork_WithMockedOptions()
    {
        // Arrange
        var mockOptions = new Mock<IOptions<DatabaseSettings>>();
        var testSettings = new DatabaseSettings
        {
            ConnectionString = "Mocked Connection String",
            TimeoutSeconds = 99,
            EnableRetries = false,
            MaxRetries = 7
        };
        mockOptions.Setup(x => x.Value).Returns(testSettings);

        var databaseService = new DatabaseService(mockOptions.Object);

        // Act & Assert
        Assert.Equal("Mocked Connection String", databaseService.GetConnectionString());
        Assert.Equal(99, databaseService.GetTimeout());
        Assert.False(databaseService.AreRetriesEnabled());

        // Verify the mock was called
        mockOptions.Verify(x => x.Value, Times.AtLeastOnce);
    }

    [Fact]
    public async Task DatabaseService_TestConnection_ShouldWork_WithMockedOptions()
    {
        // Arrange
        var mockOptions = new Mock<IOptions<DatabaseSettings>>();
        var testSettings = new DatabaseSettings
        {
            ConnectionString = "Valid Connection String"
        };
        mockOptions.Setup(x => x.Value).Returns(testSettings);

        var databaseService = new DatabaseService(mockOptions.Object);

        // Act
        var result = await databaseService.TestConnectionAsync();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DatabaseService_TestConnection_ShouldFail_WithEmptyConnectionString()
    {
        // Arrange
        var mockOptions = new Mock<IOptions<DatabaseSettings>>();
        var testSettings = new DatabaseSettings
        {
            ConnectionString = string.Empty // Empty connection string
        };
        mockOptions.Setup(x => x.Value).Returns(testSettings);

        var databaseService = new DatabaseService(mockOptions.Object);

        // Act
        var result = await databaseService.TestConnectionAsync();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("Server=test;", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Valid Connection", true)]
    public async Task DatabaseService_TestConnection_ShouldHandleVariousConnectionStrings(string connectionString, bool expectedResult)
    {
        // Arrange
        var mockOptions = new Mock<IOptions<DatabaseSettings>>();
        var testSettings = new DatabaseSettings
        {
            ConnectionString = connectionString
        };
        mockOptions.Setup(x => x.Value).Returns(testSettings);

        var databaseService = new DatabaseService(mockOptions.Object);

        // Act
        var result = await databaseService.TestConnectionAsync();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MockedOptions_ShouldAllowCustomBehavior()
    {
        // Arrange
        var mockOptions = new Mock<IOptions<DatabaseSettings>>();
        var callCount = 0;

        // Setup mock to change behavior based on call count
        mockOptions.Setup(x => x.Value).Returns(() =>
        {
            callCount++;
            return new DatabaseSettings
            {
                ConnectionString = $"Connection_{callCount}",
                TimeoutSeconds = callCount * 10
            };
        });

        var databaseService = new DatabaseService(mockOptions.Object);

        // Act & Assert - First set of calls (counter will be 1 and 2)
        Assert.Equal("Connection_1", databaseService.GetConnectionString());
        Assert.Equal(20, databaseService.GetTimeout()); // Counter is now 2 after GetConnectionString call

        // Act & Assert - Second set of calls (counter will be 3 and 4)
        Assert.Equal("Connection_3", databaseService.GetConnectionString());
        Assert.Equal(40, databaseService.GetTimeout()); // Counter is now 4 after GetConnectionString call

        // Verify call count
        mockOptions.Verify(x => x.Value, Times.Exactly(4)); // 2 calls to GetConnectionString + 2 calls to GetTimeout
    }
}