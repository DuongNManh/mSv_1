# Platform Service

## Overview

The Platform Service is responsible for managing platform resources in the microservices architecture. It acts as the source of truth for platform data and publishes platform-related events to other services.

## Features

- CRUD operations for platforms
- Event publishing via RabbitMQ
- gRPC server implementation for platform data sync
- PostgreSQL database integration
- Swagger API documentation

## API Endpoints

- GET /api/platforms - Get all platforms
- GET /api/platforms/{id} - Get platform by ID
- POST /api/platforms - Create new platform
- PUT /api/platforms/{id} - Update platform
- DELETE /api/platforms/{id} - Delete platform

## Event Publishing

The service publishes the following events via RabbitMQ:

- PlatformPublished
- PlatformCreated
- PlatformUpdated
- PlatformDeleted

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
ENTRYPOINT ["dotnet", "PlatformService.dll"]
```

## Kubernetes Deployment

```yaml
# Service deployment with:
- HTTP port: 80
- gRPC port: 1515
- Resource limits: 512Mi memory, 500m CPU
- Health checks and readiness probes
```

## Configuration

Key settings in appsettings.json:

- Database connection string
- RabbitMQ connection
- gRPC settings
- Logging configuration

## Dependencies

- .NET 8
- Entity Framework Core
- AutoMapper
- RabbitMQ.Client
- Grpc.AspNetCore
- Swashbuckle.AspNetCore
