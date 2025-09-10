# 30 - Configuration Testing Scenarios

This sample demonstrates comprehensive testing patterns for .NET configuration, covering unit tests, integration tests, validation testing, and mocking scenarios.

## What's Covered

### 1. Configuration Binding Tests
- Testing automatic binding from configuration to POCOs
- Handling missing configuration with default values
- Complex hierarchical configuration binding
- Using `IConfiguration.Get<T>()` vs `IConfiguration.Bind()`

### 2. Options Pattern Tests
- Testing dependency injection with `IOptions<T>`
- `IOptionsMonitor<T>` for dynamic configuration changes
- `IOptionsSnapshot<T>` for scoped scenarios
- Named options for multiple configurations

### 3. Mocked Configuration Tests
- Unit testing services that depend on configuration
- Using Moq to mock `IOptions<T>`
- Testing various configuration scenarios
- Custom mock behavior for complex test cases

### 4. Validation Tests
- Data annotations validation testing
- Custom validation rules
- Options validation with dependency injection
- `ValidateOnStart()` for early validation failures

### 5. Configuration Provider Tests
- In-memory configuration testing
- Provider precedence and overrides
- JSON configuration testing
- Environment variables with prefixes
- Configuration debug view testing

## Key Testing Patterns

### Unit Testing Configuration-Dependent Services
```csharp
[Fact]
public void DatabaseService_ShouldWork_WithMockedOptions()
{
    var mockOptions = new Mock<IOptions<DatabaseSettings>>();
    var testSettings = new DatabaseSettings { /* test values */ };
    mockOptions.Setup(x => x.Value).Returns(testSettings);
    
    var service = new DatabaseService(mockOptions.Object);
    // Test service behavior with known configuration
}
```

### Integration Testing with Real Configuration
```csharp
[Fact]
public void OptionsPattern_ShouldWork_WithDependencyInjection()
{
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(configData)
        .Build();

    var services = new ServiceCollection();
    services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
    // Test with real DI container and configuration binding
}
```

### Testing Configuration Validation
```csharp
[Fact]
public void OptionsValidation_ShouldFail_WithInvalidData()
{
    services.AddOptions<DatabaseSettings>()
        .ValidateDataAnnotations()
        .Validate(settings => settings.IsValid(), "Custom validation failed")
        .ValidateOnStart();
    
    // Test that validation throws expected exceptions
}
```

## Running the Tests

```bash
cd 30-configuration-testing
dotnet test
```

## Test Organization

- **ConfigurationBindingTests**: Tests for basic configuration binding
- **OptionsPatternTests**: Tests for the options pattern and DI integration
- **MockedConfigurationTests**: Unit tests with mocked dependencies
- **ValidationTests**: Configuration validation scenarios
- **ConfigurationProviderTests**: Provider-specific testing patterns

## Best Practices Demonstrated

1. **Separation of Concerns**: Testing configuration logic separately from business logic
2. **Mock Usage**: When to use mocks vs real configuration in tests
3. **Test Data Management**: Using in-memory collections for predictable tests
4. **Validation Testing**: Ensuring configuration validation works as expected
5. **Provider Testing**: Testing different configuration sources and their behavior