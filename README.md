# Microservices Project

This project consists of two microservices built with .NET that demonstrate a microservice architecture pattern.

## Architecture Overview

The system is composed of two main services:

- Platform Service
- Command Service

These services communicate using both synchronous (HTTP/gRPC) and asynchronous (Message Bus) communication patterns.

### Platform Service

**Location**: `/PlatformService`

The Platform Service is responsible for managing platform resources. It includes:

- REST API endpoints for platform management
- Database integration with PostgreSQL (Supabase)
- Swagger/OpenAPI documentation
- Message bus publishing capabilities
- Retry policies for database connections

**Key Features**:

- API security using JWT Bearer tokens
- Database resilience with retry mechanisms
- API documentation with Swagger UI
- Asynchronous event publishing

### Command Service

**Location**: `/CommandService`

The Command Service handles commands associated with platforms. It includes:

- REST API endpoints for command management
- Event-driven architecture using message bus subscription
- PostgreSQL database integration
- Automatic platform synchronization via gRPC

**Key Features**:

- Event processing system
- Asynchronous message handling
- Platform-Command relationship management
- API security with JWT
- Swagger/OpenAPI documentation

## Technical Stack

- **Framework**: .NET 8
- **Database**: PostgreSQL
- **Documentation**: Swagger/OpenAPI
- **Communication**:
  - REST APIs
  - gRPC
  - Message Bus (Asynchronous)
- **ORM**: Entity Framework Core
- **Mapping**: AutoMapper

## Getting Started

1. Ensure you have the following prerequisites:

   - .NET 8 SDK
   - PostgreSQL database
   - Message bus system

2. Configure the connection strings in each service's configuration files

3. Run the migrations for both services

4. Start the services:

   ```bash
   # For Platform Service
   cd PlatformService
   dotnet run

   # For Command Service
   cd CommandService
   dotnet run
   ```

5. Access the Swagger documentation:
   - Platform Service: `http://localhost:{port}/swagger`
   - Command Service: `http://localhost:{port}/swagger`

## Error Handling

The services implement:

- Custom exception middleware
- Database connection retry policies
- Logging
- Standard error responses

## Infrastructure

### Docker Configuration

Each service has its own Dockerfile for containerization:

- Platform Service: Uses .NET 8 SDK for build and runtime
- Command Service: Uses .NET 8 SDK for build and runtime
- RabbitMQ: Uses official RabbitMQ 3-management image

### Kubernetes Architecture

The project uses Kubernetes for orchestration with the following components:

1. **Deployments**:

   - Platform Service (platformservice)
     - Container Port: 80 (HTTP)
     - Container Port: 1515 (gRPC)
     - Resource limits: 512Mi memory, 500m CPU
   - Command Service
   - RabbitMQ
     - Ports: 5672 (AMQP), 15672 (Management)
     - Resource limits: 512Mi memory, 500m CPU

2. **Services**:
   - Platform Service ClusterIP
   - Command Service ClusterIP
   - RabbitMQ ClusterIP and LoadBalancer
   - Ingress for external access

### Message Bus (RabbitMQ)

The services communicate asynchronously using RabbitMQ:

- **Host**: rabbitmq-clusterip-srv:5672
- **Management Portal**: Port 15672
- **Message Types**:
  - Platform Published
  - Platform Created
  - Platform Updated
  - Platform Deleted

### Communication Patterns

1. **Synchronous**:

   - REST APIs between services
   - gRPC for platform synchronization

2. **Asynchronous**:
   - RabbitMQ for event-driven communication
   - Platform events publishing/subscribing

# From DuongNManh with luv :D
