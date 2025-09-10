# 17 - Dependency Injection

Accesses configuration values through DI (`IConfiguration`).

## Run

```powershell
dotnet run --project 17-dependency-injection
```

## Key Points

- Uses `config.GetValue<T>` for typed retrieval.
- Shows nested key retrieval with colon syntax.
- Illustrates difference between building host and using root service provider.
