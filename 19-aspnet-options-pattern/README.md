# 20 - ASP.NET Core Options Pattern

Introduces binding configuration to a service (`MarkdownConverter`) in an ASP.NET Core app.

## Run

```powershell
dotnet run --project 20-aspnet-options-pattern
```

Browse to the app (default: <https://localhost:5001> or assigned port).

## Key Points

- Uses `builder.Services.Configure<T>` for options.
- Demonstrates service lifetime swapping (transient/scoped/singleton commented).
- Standard minimal hosting pattern.
