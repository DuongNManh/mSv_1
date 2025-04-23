# Command Service

## Overview

The Command Service manages commands associated with platforms in the microservices architecture. It acts as a consumer of platform events and maintains its own synchronized copy of platform data.

## Features

- CRUD operations for commands
- Event subscription via RabbitMQ
- gRPC client for platform data sync
- PostgreSQL database integration
- Swagger API documentation

## API Endpoints

- GET /api/c/platforms/{platformId}/commands - Get all commands for a platform
- GET /api/c/platforms/{platformId}/commands/{commandId} - Get specific command
- POST /api/c/platforms/{platformId}/commands - Create command for platform

## Event Processing

The service subscribes to the following platform events via RabbitMQ:

- PlatformPublished (Creates/Updates platform data)
- PlatformUpdated (Updates platform data)
- PlatformDeleted (Removes platform data)

## Docker Support

```dockerfile
# Build from .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "CommandService.dll"]
```

## Kubernetes Deployment

```yaml
# Service deployment with:
- HTTP port: 80
- Resource limits: 512Mi memory, 500m CPU
- Health checks and readiness probes
```

## Data Synchronization

- Initial sync via gRPC client on startup
- Ongoing sync via RabbitMQ event subscription
- Eventual consistency model with platforms data

## Configuration

Key settings in appsettings.json:

- Database connection string
- RabbitMQ connection
- Platform service gRPC endpoint
- Logging configuration

## Dependencies

- .NET 8
- Entity Framework Core
- AutoMapper
- RabbitMQ.Client
- Grpc.Net.Client
- Swashbuckle.AspNetCore
