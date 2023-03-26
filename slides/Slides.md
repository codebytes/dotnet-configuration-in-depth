---
marp: true
theme: default
footer: 'https://chris-ayers.com'
style: |
  .columns {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 1rem;
  }
  .columns3 {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 1rem;
  } 
  img[alt~="center"] {
    display: block;
    margin: 0 auto;
  }
  table {
    width:100%;
  }
  
  @import 'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.2.1/css/all.min.css'

---
<!-- footer: 'https://github.com/codebytes/dotnet-configuration-in-depth' -->

![bg left](./img/dotnet-logo.png)

# .NET Configuration in Depth
## Chris Ayers

---

![bg left:40%](./img/portrait.png)

## Chris Ayers
### Senior Customer Engineer<br>Microsoft

<i class="fa-brands fa-twitter"></i> Twitter: @Chris\_L\_Ayers
<i class="fa-brands fa-mastodon"></i> Mastodon: @Chrisayers@hachyderm.io
<i class="fa-brands fa-linkedin"></i> LinkedIn: - [chris\-l\-ayers](https://linkedin.com/in/chris-l-ayers/)
<i class="fa fa-window-maximize"></i> Blog: [https://chris-ayers\.com/](https://chris-ayers.com/)
<i class="fa-brands fa-github"></i> GitHub: [Codebytes](https://github.com/codebytes)

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

* Limited to Key\-Value string pairs
* Accessed through a static ConfigurationManager Class
* Dependency Injection was not provided out of the box
* Transformation through difficult syntax
  * Slow Cheetah
* Hard to unit test
* Easy to leak secrets

---

# XML, Static Classes, and Parsing

<div class="columns">
<div>

![height:400px](./img/net-framework-config.png)

</div>
<div>

![height:400px](./img/net-framework-static.png)

</div>
</div>

---

# .NET 6/ASP.NET Configuration


![bg left fit 90%](./img/gears.png)

---

# Configuration Providers

![center width:980](./img/configuration-providers.png)

---

# Order Matters


![center width:980](./img/configuration-source.png)

---

# Keys are flattened

<div class="columns">
<div>

![width:500px](./img/json-config.png)

</div>
<div>

![width:500px](./img/config-flattened.png)

</div>
</div>

---

# Binding a Section

<div class="columns">
<div>

![width:500px](./img/class-binding.png)

</div>
<div>

![width:500px](./img/bind-section.png)

</div>
</div>

---

# Out of the Box

<div class="columns">
<div>

## <i class="fa fa-terminal"></i> Console
- No Configuration 

</div>
<div>

## <i class="fa-regular fa-window-maximize"></i> ASP.NET
- JSON
  - appsettings.json
  - appsettings.{Environment}.json
- Environment Variables
- Command Line Variables
- User Secrets

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

<div class="columns">
<div>

![width:500px](./img/json-config.png)

</div>
<div>

![width:500px](./img/json-provider.png)

</div>
</div>

---

# Xml Provider

<div class="columns">
<div>

![width:500px](./img/xml-config.png)

</div>
<div>

![width:500px](./img/xml-provider.png)

</div>
</div>

---

# Environment Variables

<div class="columns">
<div>

![width:500px](./img/env-config.png)

</div>
<div>

![width:500px](./img/env-provider.png)

</div>
</div>

---

# Command Line Variables

![width:500px](./img/cmdline-provider.png)


---

# Key-per-file

![width:500px](./img/keyperfile-provider.png)

---

# In Memory

![width:500px](./img/inmemory-provider.png)

---

# DEMOS

---

# The Options Pattern

<div class="columns">
<div>

The **Interface Segregation Principle (ISP)**  or Encapsulation: Scenarios (classes) that depend on configuration settings depend only on the configuration settings that they use.

</div>
<div>

**Separation of Concerns** : Settings for different parts of the app aren't dependent or coupled to one another.

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

#### GitHub Repo
#### https://github.com/codebytes/dotnet-configuration-in-depth

#### Blog
#### https://chris-ayers.com

</div>

<div>

## Contact

<i class="fa-brands fa-twitter"></i> Twitter: @Chris\_L\_Ayers
<i class="fa-brands fa-mastodon"></i> Mastodon: @Chrisayers@hachyderm.io
<i class="fa-brands fa-linkedin"></i> LinkedIn: - [chris\-l\-ayers](https://linkedin.com/in/chris-l-ayers/)
<i class="fa fa-window-maximize"></i> Blog: [https://chris-ayers\.com/](https://chris-ayers.com/)
<i class="fa-brands fa-github"></i> GitHub: [Codebytes](https://github.com/codebytes)

</div>
</div>