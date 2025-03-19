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

# DEMOS

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
