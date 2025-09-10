# 28 - .NET Aspire Basic Configuration

This sample demonstrates basic .NET Aspire configuration patterns including orchestration, service defaults, and centralized configuration management.

## What's Covered

### 1. Aspire Orchestration
- AppHost project for service orchestration
- Service discovery and communication
- Centralized configuration management

### 2. Service Defaults
- Shared configuration patterns
- Common service extensions
- Telemetry and health checks

### 3. Multi-Project Configuration
- Configuration sharing across services
- Environment-specific settings
- Aspire-specific configuration patterns

## Project Structure

```
28-aspire-basic/
├── AspireBasic.ApiService/     # API service with configuration
├── AspireBasic.AppHost/        # Orchestration host
├── AspireBasic.ServiceDefaults/# Shared service configuration
├── AspireBasic.Web/           # Web frontend
└── AspireBasic.sln            # Solution file
```

## Running the Sample

```bash
cd 28-aspire-basic
dotnet run --project AspireBasic.AppHost
```

This will start the Aspire dashboard and launch all configured services.

## Key Concepts

- **AppHost**: Central orchestration point for all services
- **Service Defaults**: Common configuration shared across services
- **Service Discovery**: Automatic service-to-service communication
- **Configuration Management**: Centralized and distributed configuration patterns

## Configuration Sources

- appsettings.json files in each service
- Environment variables
- Aspire-specific configuration providers
- Service-to-service configuration sharing
