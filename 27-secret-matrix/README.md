# 27 - Secret Strategy Matrix

Illustrates common secret storage strategies and precedence:

| Strategy | Typical Use | Local Dev Experience | Rotation Difficulty | Risk of Commit | Best For |
|----------|-------------|----------------------|--------------------|---------------|----------|
| appsettings*.json | Non-secret defaults | Immediate | N/A (not for secrets) | High | Non-sensitive defaults |
| User Secrets | Local dev secrets | Easy (dotnet CLI) | Manual | Low (outside repo) | API keys, test creds |
| Environment Variables | Container/host injection | Easy via shell | Varies (depends on platform) | Medium | Per-env secrets, ephemeral overrides |
| KeyPerFile (Docker secrets) | Container orchestrators | Simple (folder files) | Medium | Low (mounted) | Docker/K8s secrets injection |
| Azure Key Vault | Central secret mgmt | Requires Azure auth | Easy (versioned) | Low | Production secrets |
| Azure App Configuration (Key Vault Ref) | Indirect secret ref | Same as Key Vault once configured | Easy | Low | Centralized config referencing vaulted secrets |
| Command Line | One-off overrides | Manual | N/A | None (transient) | Testing precedence |

This demo focuses on: JSON, User Secrets, KeyPerFile, Environment Variables, Command Line.

Keys used:

* `Sample:PlainSecret`
* `Sample:ApiKey`
* `Sample:ConnString`

## Run Baseline

```pwsh
dotnet run --project 27-secret-matrix
```

## Apply Strategies

User Secrets:

```pwsh
dotnet user-secrets init --project 27-secret-matrix
dotnet user-secrets set "Sample:ApiKey" "From User Secrets" --project 27-secret-matrix
```

KeyPerFile (creates `secrets/PlainSecret`):

```pwsh
mkdir secrets | Out-Null
Set-Content secrets/PlainSecret "From KeyPerFile file"
```

Environment Variable:

```pwsh
$env:Sample__PlainSecret = "From Env Var"
```

Command Line Override:

```pwsh
dotnet run --project 27-secret-matrix -- --Sample:PlainSecret FromCmdLine
```

Re-run to inspect after each step:

```pwsh
dotnet run --project 27-secret-matrix --no-build
```

Cleanup:

```pwsh
Remove-Item Env:Sample__PlainSecret -ErrorAction SilentlyContinue
Remove-Item secrets -Recurse -Force -ErrorAction SilentlyContinue
```

## Teaching Points

* Later providers override earlier ones (command line last ⇒ highest precedence).
* User Secrets only load in Development and never for production.
* KeyPerFile maps filename to key; folder mounting is common for Docker secrets.
* Prefer vault / managed secret stores for production (Key Vault, etc.).
