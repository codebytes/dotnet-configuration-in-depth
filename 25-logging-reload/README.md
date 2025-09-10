# 25 - Logging Level Live Reload

Demonstrates dynamic reload of logging levels using `reloadOnChange` for `appsettings.json`.

## Run

```pwsh
dotnet run --project 20-logging-reload
```

While it runs, open `20-logging-reload/appsettings.json` and change:

```json
"Default": "Trace"
```

Save the file and observe additional log lines (Trace/Debug) begin to appear without restarting the process.

## Key Teaching Points

* The logging system listens for configuration reload token changes.
* Lowering the log level (e.g., Information -> Trace) increases output immediately.
* Raising the level reduces noise in real time.
* Useful for operational diagnostics without redeploying.
