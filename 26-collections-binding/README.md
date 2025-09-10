# 26 - Collections & Required Member Binding

Demonstrates binding of complex collections (lists + dictionaries) and C# `required` members, along with DataAnnotations validation + fail-fast (`ValidateOnStart`).

## Highlights

- Arrays / lists of POCOs (`Warehouses`)
- Nested dictionaries (`Bins` inside each warehouse)
- Primitive collections (`ApiKeys`), dictionary of primitives (`FeatureFlags`)
- `required` keyword to surface missing configuration keys
- DataAnnotations: `[Required]`, `[Url]`, `[MinLength]`
- `ValidateOnStart()` triggers early failure instead of deferred access

## Run

```powershell
dotnet run --project 18-config-collections
```

You should see the inventory + required settings printed.

## Experiment

1. Remove `Endpoint` or change it to `not-a-url` and re-run -> validation error.
2. Remove entire `RequiredSettings` section -> aggregated validation failures.
3. Add env vars with prefix `COLLECTIONS_` to override values, e.g. (PowerShell):

```powershell
$env:COLLECTIONS_RequiredSettings__Endpoint="https://override.example.com"
$env:COLLECTIONS_RequiredSettings__ApiKeys__0="kX"
```

Then run again.

## Teaching Points

- Missing required members cause validation failure when using `[Required]` + `required` + `ValidateOnStart`.
- Collections binding: Index-based for arrays/lists; key names for dictionaries.
- Environment variable override uses `__` to denote nesting.
