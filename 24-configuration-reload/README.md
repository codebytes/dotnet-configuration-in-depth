# ReloadMonitor Demo

Demonstrates dynamic configuration reload using `IOptionsMonitor<T>`.

## Key Points

- `AddJsonFile` with `reloadOnChange: true`.
- `IOptionsMonitor<T>.OnChange` subscription prints updated values.
- Shows difference between periodic read of `CurrentValue` and change callback.

## Try It

1. Run the app.
2. Edit `appsettings.json` (change `Greeting`, `DelaySeconds`, or `Feature` values) and save.
3. Observe `[Reload]` lines emitted without restarting.

## Sample Change

```json
{
  "MySettings": {
    "Greeting": "Hello (updated)",
    "DelaySeconds": 1,
    "Feature": { "Enabled": false, "Threshold": 9 }
  }
}
```
