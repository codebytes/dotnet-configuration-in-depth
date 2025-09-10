# 13 - Named Options

Shows multiple named `FileOptions` registrations for `pdf`, `doc`, and `html` sections.

## Run

```powershell
dotnet run --project 13-named-options
```

## Key Points

- `Configure<FileOptions>("name", section)` pattern.
- Enables different configuration snapshots per logical name.
