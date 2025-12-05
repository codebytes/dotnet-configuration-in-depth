## Overview

- Explores hierarchical sections such as Greeting:Message.
- Shows how nested keys flow from JSON, environment variables, and command-line arguments.

## Run

```powershell
dotnet run --project 08-configuration-sections -- --greeting:color=blue
```

### Additional Examples

```bash
dotnet run --project 08-configuration-sections -- --greeting:color=green
dotnet run --project 08-configuration-sections/ConfigurationSections.csproj
```
