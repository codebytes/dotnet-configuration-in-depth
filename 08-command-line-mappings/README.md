# 08 - Command Line Mappings

Adds a custom switch mapping dictionary to map friendly switches to config keys.

## Run

```powershell
dotnet run --project 08-command-line-mappings -- --color=green --greeting="Hi"
```

### Additional Examples

```bash
dotnet run --project 08-command-line-mappings -- --greeting:color=blue
dotnet run --project 08-command-line-mappings -- --color=blue
```

## Key Points

- Demonstrates `AddCommandLine(args, mappings)`.
- Enables shorter / domain-friendly switches.
- Both mapped and raw hierarchical syntax work.
