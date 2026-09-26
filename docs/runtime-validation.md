# Stable .NET runtime validation

All 32 sample projects target .NET 10. `global.json` selects SDK 10.0.401; core Microsoft.Extensions and ASP.NET Core packages are aligned to the 10.0.12 servicing release. The previous Azure.Identity and Aspire SDK alignment fixes are preserved.

The new `.NET build and tests` workflow restores and compiles the full solution, runs the configuration unit tests and integration tests, then executes the native Aspire test runner against Docker on the disposable GitHub-hosted VM. It has read-only repository permissions, no persisted checkout credentials, and no cloud credentials or deployment steps.

The integration test keeps its strict 200 ms assertion. It first warms the real endpoint to exclude variable routing/JIT cold-start overhead from the configured-delay measurement. A validation-only negative control with the default 300 ms delay must still fail that same assertion. Container tests never require a personal machine or Gateway Docker socket. Azure deployment and secret configuration are outside this build workflow.

The CI runner generates/trusts its own local development HTTPS certificate before Aspire starts and checks that trust explicitly. TLS certificate validation remains enabled. No certificate or trust store on the Gateway, a personal machine, or a cloud service is changed.
The SDK-required OpenSSL development trust directory is included in `SSL_CERT_DIR` alongside the existing system roots for this CI job only.
