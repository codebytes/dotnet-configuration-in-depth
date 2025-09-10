# 31 - Integration Testing with Configuration

This sample demonstrates how to perform integration testing of ASP.NET Core applications with configuration management, including custom test configurations and environment-specific settings.

## What's Covered

### 1. Basic Integration Tests
- Testing web API endpoints that depend on configuration
- Verifying configuration is loaded correctly in test environment
- Using `WebApplicationFactory<T>` for integration testing

### 2. Custom Test Configuration
- Overriding configuration for tests using `CustomWebApplicationFactory`
- Adding in-memory configuration for test scenarios
- Environment-specific configuration files (appsettings.Test.json)

### 3. Configuration-Dependent Service Testing
- Testing services that use `IOptions<T>` in integration context
- Verifying configuration binding works end-to-end
- Performance testing with different configuration values

## Key Patterns

### Standard Integration Test
```csharp
public class WebApplicationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task ApiSettings_ShouldBeAccessible_ThroughEndpoint()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/settings");
        // Verify configuration is accessible through API
    }
}
```

### Custom Test Factory with Configuration Override
```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(testConfigData);
        });
    }
}
```

### Testing Configuration Loading
```csharp
[Fact]
public void Configuration_ShouldBeLoadedCorrectly_InTestEnvironment()
{
    var configuration = _factory.Services.GetRequiredService<IConfiguration>();
    Assert.Equal("Test", configuration["Api:Environment"]);
}
```

## Project Structure

```
31-integration-testing/
├── WebApp/
│   └── Program.cs              # Simple web API with configuration
├── Tests/
│   └── WebApplicationIntegrationTests.cs  # Integration test examples
├── appsettings.json            # Default configuration
├── appsettings.Test.json       # Test environment configuration
└── README.md
```

## Running the Tests

```bash
cd 31-integration-testing
dotnet test
```

## Configuration Files

### appsettings.json (Default)
- Production/development settings
- Normal timeout values and logging levels

### appsettings.Test.json (Test Environment)
- Test-specific overrides
- Faster timeouts for quicker tests
- Reduced logging to minimize test noise

## Best Practices Demonstrated

1. **Environment-Specific Configuration**: Using appsettings.Test.json for test-specific settings
2. **Custom Test Factories**: Creating specialized test environments with configuration overrides
3. **In-Memory Configuration**: Adding test data without external files
4. **Configuration Verification**: Testing that configuration is loaded and applied correctly
5. **Performance Testing**: Using different configuration values to test performance scenarios
6. **Service Integration**: Testing that configuration flows correctly through dependency injection

## Integration vs Unit Testing

This sample shows **integration testing** where:
- The full application pipeline is tested
- Real configuration loading is used
- HTTP endpoints are tested end-to-end
- Services interact through real dependency injection

Compare with unit testing (sample 30) where:
- Individual components are tested in isolation
- Configuration is mocked or provided directly
- No HTTP pipeline is involved