# 07 - User Secrets

Demonstrates `AddUserSecrets<Program>()` for local secret storage (development only).

## Setup

```powershell
dotnet user-secrets init --project 07-user-secrets
dotnet user-secrets set "greeting" "SecretHello" --project 07-user-secrets
```

## Run

```powershell
dotnet run --project 07-user-secrets
```

## Key Points

- User secrets override JSON but are not checked into source.
- Suitable for API keys during local dev; not for production.
