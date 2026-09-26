# Stable .NET runtime validation

All 32 sample projects target .NET 10. `global.json` selects SDK 10.0.401; core Microsoft.Extensions and ASP.NET Core packages are aligned to the 10.0.12 servicing release. The previous Azure.Identity and Aspire SDK alignment fixes are preserved.

The new `.NET build and tests` workflow restores and compiles the full solution, runs the configuration unit tests and integration tests, then executes the native Aspire test runner against Docker on the disposable GitHub-hosted VM. It has read-only repository permissions, no persisted checkout credentials, and no cloud credentials or deployment steps.

The existing integration test's strict 200 ms assertion is unchanged. Container tests never require a personal machine or Gateway Docker socket. Azure deployment and secret configuration are outside this build workflow.
