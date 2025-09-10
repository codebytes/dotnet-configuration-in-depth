# 06 - In-Memory Configuration

Uses `AddInMemoryCollection` to seed configuration dynamically at runtime.

## Run

```powershell
dotnet run --project 06-in-memory-config
```

## Key Points

- Great for tests or programmatic seeding.
- Shows accessing a key not present (`SomeKey`) returning null.
- Prepares for binding + sections in later demos.
