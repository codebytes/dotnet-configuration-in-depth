# 09 - Configuration Sections

Demonstrates hierarchical keys (e.g. `greeting:message`) and retrieving nested values.

## Run

```powershell
dotnet run --project 09-configuration-sections -- --greeting:color=blue
```

### Additional Examples

```bash
dotnet run --project 09-configuration-sections -- --greeting:color=green
```

## Key Points

- Shows colon-delimited keys for hierarchy.
- Mixes JSON + environment + (optional) user secrets + command line.
- Basis for section binding in later samples.
