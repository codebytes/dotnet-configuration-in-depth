# 29 - Aspire Advanced Layering

Demonstrates configuration layering in a .NET Aspire distributed app:

Layers (last wins inside each process):

1. SharedConfig project `appsettings.json`
2. Service project `appsettings.json`
3. `appsettings.{Environment}.json`
4. User secrets (if configured; see UserSecretsId in csproj)
5. Environment variables injected by AppHost (`WithEnvironment` calls / parameters)
6. Command line / parameters overrides

AppHost defines parameters:

- `ApiBaseMessage` -> injected into API as `Api:InjectedMessage`
- `WorkerIntervalSeconds` -> injected into worker as `Worker:IntervalSeconds`
- `ExternalApiKey` -> secret placeholder (override via `setx ExternalApiKey <value>` or user secrets)

Try overriding at run time:

```bash
# Override parameter at launch
DOTNET_ExternalApiKey=SUPERSECRET dotnet run --project 23-aspire-layering/23-aspire-layering.AppHost \
    -- -p:ApiBaseMessage="Override from CLI" -p:WorkerIntervalSeconds=2
```

API endpoint `/config` returns effective values plus provider list.
Worker logs show interval, shared message, and last 4 chars of secret.

Experiment:

1. Change `23-aspire-layering.SharedConfig/appsettings.json` SharedMessage -> observe both services after restart.
2. Add `Api:InjectedMessage` to API `appsettings.Development.json` -> observe environment override.
3. Provide env var `Api__InjectedMessage` -> takes precedence.
4. Provide command line parameter override via AppHost parameter flag.

Security: secret printed only masked (last 4 chars). Avoid storing real secrets in JSON; use environment or secret store.
