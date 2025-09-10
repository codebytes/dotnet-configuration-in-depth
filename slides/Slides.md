---
marp: true
theme: custom-default
footer: 'https://chris-ayers.com'
---

<!-- _footer: 'https://github.com/codebytes/dotnet-configuration-in-depth' -->

# .NET Configuration in Depth

## Chris Ayers

![bg left](./img/dotnet-logo.png)

---

![bg left:40%](./img/portrait.png)

## Chris Ayers

### Senior Risk SRE<br>Azure CXP AzRel<br>Microsoft

<i class="fa-brands fa-bluesky"></i> BlueSky: [@chris-ayers.com](https://bsky.app/profile/chris-ayers.com)
<i class="fa-brands fa-linkedin"></i> LinkedIn: - [chris\-l\-ayers](https://linkedin.com/in/chris-l-ayers/)
<i class="fa fa-window-maximize"></i> Blog: [https://chris-ayers\.com/](https://chris-ayers.com/)
<i class="fa-brands fa-github"></i> GitHub: [Codebytes](https://github.com/codebytes)
<i class="fa-brands fa-mastodon"></i> Mastodon: [@Chrisayers@hachyderm.io](https://hachyderm.io/@Chrisayers)
~~<i class="fa-brands fa-twitter"></i> Twitter: @Chris_L_Ayers~~

---

![bg right:40% auto](./img/dotnet-logo.png)

# Agenda

- What is configuration?
- How does .NET Framework handle configuration?
- How does .NET and ASP.NET handle configuration?
- Configuration Providers
- Configuration Binding
- The Options Pattern
- Questions?

---

## What is Configuration?

<div class="columns3">
<div>

## <i class="fa fa-sliders"></i> Settings

- Retry Times
- Queue Length

</div>
<div>

## <i class="fa-sharp fa-solid fa-flag"></i> Feature Flags

- Per User
- Percentage

</div>
<div>

## <i class="fa fa-key"></i> Secrets

- Connection Strings
- App Registration

</div>
</div>

---

# When is configuration applied?

<div class="columns">
<div>

## <i class="fa fa-code"></i> Compile Time

![height:400px](./img/compile-time.png)

</div>
<div>

## <i class="fa-solid fa-file-code"></i> Run Time

![height:400px](./img/run-time.png)

</div>
</div>

---

# .NET Framework Configuration

![bg right fit 90%](./img/coding.png)

---

# Web.Config

- Limited to Key\-Value string pairs
- Accessed through a static ConfigurationManager Class
- Dependency Injection was not provided out of the box
- Transformation through difficult syntax
  - Slow Cheetah
- Hard to unit test
- Easy to leak secrets

---

# XML, Static Classes, and Parsing

<div class="columns">
<div>

```xml
  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
    <add key="Greeting" value="Hello, Everyone!" />
    <add key="CurrentMajorDotNetVersion" value="6" />
  </appSettings>
  <system.web>
    <compilation debug="true" targetFramework="4.7.2" />
    <httpRuntime targetFramework="4.7.2" />
  </system.web>
```

</div>
<div>

```xml
<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
  <connectionStrings>
    <add name="MyDB"
      connectionString="Data Source=ReleaseSQLServer..."
      xdt:Transform="SetAttributes" xdt:Locator="Match(name)"/>
  </connectionStrings>

```

```csharp
  private string greeting = "";
  private int majorDotNetVersion = 0;
  public HomeController()
  {
      greeting = ConfigurationManager.AppSettings["Greeting"];
      string ver = ConfigurationManager.AppSettings["CurrentMajorDotNetVersion"]
      majorDotNetVersion = Int32.Parse(ver);
  }
```

</div>
</div>

---

# .NET Core/5/6/7/8 <br /> ASP.NET Configuration

![bg left fit 90%](./img/gears.png)

---

# Configuration Providers

![center width:980](./img/configuration-providers.png)

---

# Order Matters

![center width:980](./img/configuration-source.png)

---

# Binding

<div class="columns">
<div>

```json
{
    "Settings": {
        "KeyOne": 1,
        "KeyTwo": true,
        "KeyThree": {
            "Message": "Oh, that's nice...",
            "SupportedVersions": {
                "v1": "1.0.0",
                "v3": "3.0.7"
            }
        }
    }
}
```

</div>
<div>

```csharp
public sealed class Settings
{
    public required int KeyOne { get; set; }
    public required bool KeyTwo { get; set; }
    public required NestedSettings KeyThree { get; set; };
}
public sealed class NestedSettings
{
    public required string Message { get; set; };
}

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
    
Settings? settings = 
  config.GetRequiredSection("Settings")
    .Get<Settings>();

```

</div>
</div>

---

# Hierarchical Configuration Data

## Keys are Flattened

<div class="columns">
<div>

```json
{
  "Parent": {
    "FavoriteNumber": 7,
    "Child": {
      "Name": "Example",
      "GrandChild": {
        "Age": 3
      }
    }
  }
}
```

</div>
<div>

```json
{
  "Parent:FavoriteNumber": 7,
  "Parent:Child:Name": "Example",
  "Parent:Child:GrandChild:Age": 3
}
```

</div>
</div>

---

# Out of the Box

<div class="columns3">
<div>

## <i class="fa fa-terminal"></i> Console

- No Configuration

</div>
<div>

## .NET Generic Host

- JSON
  - appsettings.json
  - appsettings.{Environment}.json
- User Secrets
- Environment Variables
- Command Line Variables

</div>
<div>

## <i class="fa-regular fa-window-maximize"></i> ASP.NET

- JSON
  - appsettings.json
  - appsettings.{Environment}.json
- User Secrets
- Environment Variables
- Command Line Variables

</div>
</div>

---

# Configuration Providers

<div class="columns">
<div>

## File-based

- JSON
- XML
- INI
- Key-per-file

</div>
<div>

## Others

- Environment variables
- Command-line
- In-Memory
- User Secrets
- Azure Key Vault
- Azure App Configuration

</div>
</div>

---

# Json Provider

<div class="columns21">
<div>

```xml
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
```

```csharp
IHostEnvironment env = builder.Environment;

builder.Configuration
  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
```

</div>
<div>

```json
{
    "SecretKey": "Secret key value",
    "TransientFaultHandlingOptions": {
        "Enabled": true,
        "AutoRetryDelay": "00:00:07"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    }
}
```

</div>
</div>

---

# XML Provider

<div class="columns21">
<div>

```xml
<PackageReference Include="Microsoft.Extensions.Configuration.Xml" Version="8.0.0" />
```

```csharp
IHostEnvironment env = builder.Environment;

builder.Configuration
  .AddXmlFile("appsettings.xml", optional: true, reloadOnChange: true)
  .AddXmlFile("repeating-example.xml", optional: true, reloadOnChange: true);
```

</div>
<div>

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <SecretKey>Secret key value</SecretKey>
  <TransientFaultHandlingOptions>
    <Enabled>true</Enabled>
    <AutoRetryDelay>00:00:07</AutoRetryDelay>
  </TransientFaultHandlingOptions>
  <Logging>
    <LogLevel>
      <Default>Information</Default>
      <Microsoft>Warning</Microsoft>
    </LogLevel>
  </Logging>
</configuration>
```

</div>
</div>

---

# INI Provider

<div class="columns21">
<div>

```xml
<PackageReference Include="Microsoft.Extensions.Configuration.Ini" Version="8.0.0" />
```

```csharp
IHostEnvironment env = builder.Environment;

builder.Configuration
  .AddIniFile("appsettings.ini", optional: true, reloadOnChange: true)
  .AddIniFile($"appsettings.{env.EnvironmentName}.ini", true, true);
```

</div>
<div>

```ini
SecretKey="Secret key value"

[TransientFaultHandlingOptions]
Enabled=True
AutoRetryDelay="00:00:07"

[Logging:LogLevel]
Default=Information
Microsoft=Warning
```

</div>
</div>

---

# Environment Variables

<div class="columns">
<div>

- Typically used to override settings found in appsettings.json or user secrets
- the : delimiter doesn't work for Hierarchical data on all platforms
- the `__` delimiter is used instead of `:`

</div>
<div>

```csharp
public class TransientFaultHandlingOptions
{
    public bool Enabled { get; set; }
    public TimeSpan AutoRetryDelay { get; set; }
}
```

```bash
set SecretKey="Secret key from environment"
set TransientFaultHandlingOptions__Enabled="true"
set TransientFaultHandlingOptions__AutoRetryDelay="00:00:13"
```

</div>
</div>

---

# Environment Variables

<div class="columns">
<div>

- There are built-in prefixes, like
  - ASPNETCORE_ for ASP.NET Core
  - DOTNET_ for .NET Core
- You can provide your own prefix
  
</div>
<div>

```csharp
builder.Configuration.AddEnvironmentVariables(
  prefix: "MyCustomPrefix_");
```

```bash
set MyCustomPrefix_MyKey="My key with MyCustomPrefix_"
set MyCustomPrefix_Position__Title=Editor_with_custom
set MyCustomPrefix_Position__Name=Environment
dotnet run
```

</div>
</div>

---

# DEMOS

---

# The Options Pattern

<div class="columns">
<div>

**Interface Segregation Principle (ISP)**: Scenarios (classes) that depend on configuration settings depend only on the configuration settings that they use.

</div>
<div>

**Separation of Concerns** : Settings for different parts of the app aren't dependent or coupled to one another.

</div>
</div>

---

# An Options Class

<div class="columns">
<div>

- Must be non-abstract with a public parameterless constructor
- Contain public read-write properties to bind (fields are not bound)

</div>
<div>

```csharp
public class FileOptions
{
    public string FileExtension { get; set; } ="";
    public string OutputDir { get; set; } ="";
    public string TemplateFile { get; set; } ="";
}
```

</div>
</div>

---

# Types of IOptions

|  | Singleton | Reloading Support | Named Option Support |
| :-: | :-: | :-: | :-: |
| IOptions<T><br /> | Yes | No | No |
| IOptionsSnapshot<T><br /> | No | Yes | Yes |
| IOptionsMonitor<T><br /> | Yes | Yes | Yes |

---

# Performance: IOptionsSnapshot vs IOptionsMonitor

<div class="columns">
<div>

## <i class="fa fa-camera"></i> IOptionsSnapshot&lt;T&gt;

- **Scoped lifetime** (per request)
- **Recomputed each request**
- **Supports named options**
- **Best for web apps**

```csharp
public class MyController : Controller
{
    public MyController(IOptionsSnapshot<MyOptions> options)
    {
        // Fresh config per request
        _options = options.Value;
    }
}
```

</div>
<div>

## <i class="fa fa-tv"></i> IOptionsMonitor&lt;T&gt;

- **Singleton lifetime**
- **Real-time change notifications**
- **Supports named options**
- **Best for background services**

```csharp
public class MyService : BackgroundService
{
    public MyService(IOptionsMonitor<MyOptions> monitor)
    {
        // React to config changes
        monitor.OnChange(OnConfigChanged);
    }
}
```

</div>
</div>

---

# Testing Configuration

---

# Configuration Testing Patterns

<div class="columns">
<div>

## <i class="fa fa-flask"></i> Unit Testing

```csharp
[Test]
public void Service_Uses_Configuration_Correctly()
{
    // Arrange
    var config = new ConfigurationBuilder()
        .AddInMemoryCollection(new[]
        {
            new KeyValuePair<string, string>("ApiUrl", "https://test.api"),
            new KeyValuePair<string, string>("Timeout", "30")
        })
        .Build();
    
    var options = Options.Create(config.Get<ApiOptions>());
    var service = new ApiService(options);
    
    // Act & Assert
    Assert.That(service.BaseUrl, Is.EqualTo("https://test.api"));
}
```

</div>
<div>

## <i class="fa fa-cog"></i> Integration Testing

```csharp
public class TestWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("Database:ConnectionString", 
                    "Server=localhost;Database=TestDb;"),
                new KeyValuePair<string, string>("ExternalApi:BaseUrl", 
                    "https://mock-api.test")
            });
        });
    }
}
```

</div>
</div>

---

# Configuration Validation

<div class="columns">
<div>

## <i class="fa fa-check-circle"></i> Data Annotations

```csharp
public class DatabaseOptions
{
    [Required]
    [Url]
    public string ConnectionString { get; set; } = "";
    
    [Range(1, 300)]
    public int CommandTimeoutSeconds { get; set; } = 30;
    
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_]+$")]
    public string DatabaseName { get; set; } = "";
}

// Register with validation
services.AddOptions<DatabaseOptions>()
    .Bind(configuration.GetSection("Database"))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

</div>
<div>

## <i class="fa fa-shield-alt"></i> Custom Validation

```csharp
public class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
{
    public ValidateOptionsResult Validate(string name, DatabaseOptions options)
    {
        var failures = new List<string>();
        
        if (string.IsNullOrEmpty(options.ConnectionString))
            failures.Add("ConnectionString is required");
            
        if (options.CommandTimeoutSeconds <= 0)
            failures.Add("CommandTimeoutSeconds must be positive");
            
        if (!IsValidDatabaseName(options.DatabaseName))
            failures.Add("Invalid database name format");
            
        return failures.Count > 0 
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}

// Register validator
services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseOptionsValidator>();
```

</div>
</div>

---

# Validation at Startup

<div class="columns">
<div>

## <i class="fa fa-rocket"></i> Fail Fast Pattern

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configure options with validation
builder.Services.AddOptions<ApiOptions>()
    .Bind(builder.Configuration.GetSection("Api"))
    .ValidateDataAnnotations()
    .ValidateOnStart(); // Validates during app startup

builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection("Database"))
    .Validate(options => !string.IsNullOrEmpty(options.ConnectionString), 
              "Connection string cannot be empty")
    .ValidateOnStart();

var app = builder.Build();
// App fails to start if validation fails
```

</div>
<div>

## <i class="fa fa-exclamation-triangle"></i> Benefits

- **Early Detection**: Catch configuration errors at startup
- **Clear Error Messages**: Know exactly what's wrong
- **Prevents Runtime Failures**: No surprises in production
- **Better DevEx**: Immediate feedback during development

```csharp
// Custom validation method
services.AddOptions<MyOptions>()
    .Bind(configuration.GetSection("MySection"))
    .Validate(options => 
    {
        return options.ApiKey?.Length >= 10;
    }, "ApiKey must be at least 10 characters")
    .ValidateOnStart();
```

</div>
</div>

---

# DEMOS

---

# Secrets Management Best Practices

<div class="columns">
<div>

## <i class="fa fa-exclamation-triangle"></i> Don't

- Store secrets in appsettings.json
- Commit secrets to source control
- Use production secrets in development
- Log configuration values containing secrets

</div>
<div>

## <i class="fa fa-check-circle"></i> Do

- Use User Secrets for development
- Use Azure Key Vault for production
- Use environment variables for containers
- Implement proper secret rotation
- Validate secrets at startup

</div>
</div>

---

# Secrets by Environment

<div class="columns3">
<div>

## <i class="fa fa-laptop-code"></i> Development

- **User Secrets**
  - Per-project secrets
  - Stored outside source control
  - Easy to manage locally

```bash
dotnet user-secrets set "ApiKey" "dev-key-123"
```

</div>
<div>

## <i class="fa fa-server"></i> Staging/Production

- **Azure Key Vault**
  - Centralized secret management
  - Access policies and RBAC
  - Audit logging
  - Automatic rotation

```csharp
builder.Configuration.AddAzureKeyVault(
  keyVaultUrl, credential);
```

</div>
<div>

## <i class="fa fa-cube"></i> Containers

- **Environment Variables**
  - Kubernetes secrets
  - Docker secrets
  - Service connection strings

```bash
docker run -e "ConnectionString=..." myapp
```

</div>
</div>

---

# Environment-Specific Configuration Strategies

<div class="columns">
<div>

## <i class="fa fa-layer-group"></i> Layered Configuration

```csharp
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);
```

**Order matters!** Later sources override earlier ones.

</div>
<div>

## <i class="fa fa-code-branch"></i> Environment Patterns

- **Development**: User Secrets + local files
- **Staging**: Environment variables + Key Vault
- **Production**: Key Vault + minimal env vars
- **Testing**: In-memory configuration

```csharp
if (env.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
```

</div>
</div>

---

# Configuration Security Considerations

<div class="columns">
<div>

## <i class="fa fa-shield-alt"></i> Prevent Secret Leakage

- **Never log IConfiguration directly**
- **Redact sensitive values in logs**
- **Use IOptionsSnapshot/IOptionsMonitor**
- **Implement custom configuration providers for sensitive data**

</div>
<div>

## <i class="fa fa-eye-slash"></i> Secure Logging

```csharp
// ❌ DON'T - Exposes all configuration
logger.LogInformation("Config: {Config}", 
    JsonSerializer.Serialize(configuration));

// ✅ DO - Log specific, non-sensitive values
logger.LogInformation("Database timeout: {Timeout}s", 
    dbOptions.CommandTimeout);
```

</div>
</div>

---

# .NET Aspire

---

# .NET Aspire Configuration

## Cloud-Native Configuration Made Simple

- **Orchestration**: Centralized service management through AppHost
- **Service Defaults**: Opinionated baseline for observability, health checks, and service discovery
- **Configuration Layering**: Hierarchical configuration across distributed services
- **Modern Patterns**: Built-in support for microservices and cloud-native apps

---

# Aspire Configuration Architecture

<div class="columns">
<div>

## AppHost Project

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ApiService>("apiservice");

var workerService = builder.AddProject<Projects.WorkerService>("workerservice")
    .WithEnvironment("Api:BaseUrl", apiService.GetEndpoint("https"));

builder.Build().Run();
```

</div>
<div>

## Service Defaults

```csharp
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();
        builder.AddDefaultHealthChecks();
        builder.Services.AddServiceDiscovery();
        
        builder.Services.ConfigureHttpClientDefaults(http => 
        {
            http.AddStandardResilienceHandler();
            http.UseServiceDiscovery();
        });
        
        return builder;
    }
}
```

</div>
</div>

---

# Aspire Configuration Layering

## Configuration Priority (Last Wins)

1. **SharedConfig** `appsettings.json` - Cross-service shared settings  
2. **Service-specific** `appsettings.json` - Per-service configuration  
3. **Environment-specific** `appsettings.{Environment}.json`  
4. **User Secrets** (Development only)  
5. **AppHost Environment Variables** - `WithEnvironment()` calls  
6. **Command Line Parameters** - Runtime overrides  

```csharp
// AppHost parameter injection
var apiService = builder.AddProject<Projects.ApiService>("apiservice")
    .WithEnvironment("Api:InjectedMessage", builder.AddParameter("ApiBaseMessage"));
```

---

# Questions?

---

<div class="columns">
<div>

## Resources

### GitHub Repo

- [https://github.com/codebytes/dotnet-configuration-in-depth](https://github.com/codebytes/dotnet-configuration-in-depth)

### Docs

- [.NET Configuration Docs](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)
- [.NET Aspire Docs](https://learn.microsoft.com/en-us/dotnet/aspire/)

</div>

<div>

## Contact

## Chris Ayers

<i class="fa-brands fa-bluesky"></i> BlueSky: [@chris-ayers.com](https://bsky.app/profile/chris-ayers.com)
<i class="fa-brands fa-linkedin"></i> LinkedIn: - [chris\-l\-ayers](https://linkedin.com/in/chris-l-ayers/)
<i class="fa fa-window-maximize"></i> Blog: [https://chris-ayers\.com/](https://chris-ayers.com/)
<i class="fa-brands fa-github"></i> GitHub: [Codebytes](https://github.com/codebytes)
<i class="fa-brands fa-mastodon"></i> Mastodon: [@Chrisayers@hachyderm.io](https://hachyderm.io/@Chrisayers)
~~<i class="fa-brands fa-twitter"></i> Twitter: @Chris_L_Ayers~~

</div>
</div>
