# 03 - JSON Configuration

Introduces the JSON configuration provider via `AddJsonFile("config.json")`.

## Run

```powershell
dotnet run --project 03-json-config
```

## Key Points

- Builds an `IConfigurationRoot` with a single JSON file.
- Accesses values using indexer: `configuration["greeting"]`.
- Establishes baseline for precedence demos (env vars, command line) in later samples.

## Try

Edit `config.json` and re-run to see value changes (no reload-on-change here).
