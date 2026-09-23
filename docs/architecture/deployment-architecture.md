# Deployment Architecture

## 1. Overview

This document describes the deployment architecture for the SpaceGeeks website, including packaging, deployment mechanisms, and hosting considerations.

## 2. Deployment Model

The SpaceGeeks application follows a simple deployment model as a self-contained .NET application:

```mermaid
graph LR
    A[Source Code] --> B[Build Process]
    B --> C[Self-Contained Executable]
    C --> D[Target Environment]
    
    style A fill:#4CAF50,stroke:#388E3C
    style B fill:#2196F3,stroke:#0D47A1
    style C fill:#FF9800,stroke:#E65100
    style D fill:#9C27B0,stroke:#4A148C
```

## 3. Build Process

### 3.1 Source Code Structure
The application source code is organized into two main projects:
- `SpaceGeeks` - Main web application
- `SpaceGeeks.Tests` - Test suite

### 3.2 Build Commands
The application can be built using standard .NET CLI commands:

```bash
# Build for development
dotnet build

# Publish as self-contained executable
dotnet publish -c Release -r <RID> --self-contained true

# Run tests
dotnet test
```

### 3.3 Output Artifacts
The build process produces:
- Executable binary (platform-specific)
- Static assets (wwwroot directory contents)
- Configuration files (appsettings.json)

## 4. Runtime Environment

### 4.1 Hosting Model
The application is self-hosted using Kestrel web server, which is embedded within the executable.

**Characteristics:**
- No external web server required (IIS, Apache, nginx)
- Single executable deployment
- Cross-platform compatibility
- Built-in HTTPS support

### 4.2 Resource Requirements
**Minimum Requirements:**
- CPU: 1 core
- Memory: 100MB
- Disk: 100MB for application files

**Recommended Requirements:**
- CPU: 2 cores
- Memory: 200MB
- Disk: 200MB for application files

### 4.3 Supported Platforms
The application can be deployed on any platform supported by .NET 8:
- Windows (x64, ARM64)
- Linux (x64, ARM64)
- macOS (x64, ARM64)

## 5. Configuration

### 5.1 Configuration Sources
Configuration is loaded from the following sources in order:
1. `appsettings.json` - Base configuration
2. `appsettings.{Environment}.json` - Environment-specific overrides
3. Environment variables - Runtime overrides

### 5.2 Key Configuration Settings
- **Kestrel URLs**: Configures HTTP/HTTPS binding addresses
- **Logging Levels**: Controls verbosity of application logs
- **Static File Options**: Configures caching and compression

### 5.3 Environment-Specific Configuration
Different environments (Development, Production) can have different configurations:
- Development: Detailed error pages, verbose logging
- Production: Minimal error information, standard logging

## 6. Deployment Scenarios

### 6.1 Local Development
Developers can run the application directly from source code:

```bash
cd SpaceGeeks
dotnet run
```

**Characteristics:**
- Hot reload support
- Detailed error information
- Development certificates for HTTPS

### 6.2 Standalone Server
Application deployed as a service on a dedicated server:

```bash
# Using systemd on Linux
sudo systemctl start spacegeeks.service

# Using Windows Service
sc start SpaceGeeks
```

**Characteristics:**
- Runs as background service
- Automatic restart on failure
- System-level logging integration

### 6.3 Container Deployment
Application packaged as Docker container:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ... build steps ...

ENTRYPOINT ["dotnet", "SpaceGeeks.dll"]
```

**Characteristics:**
- Consistent runtime environment
- Easy scaling and orchestration
- Platform-agnostic deployment

## 7. Monitoring and Observability

### 7.1 Health Checks
The application exposes basic health check endpoints:
- `/health` - Overall application health
- Built-in checks for web server availability

### 7.2 Logging
Structured logging is implemented using .NET's built-in logging infrastructure:
- Console output for container environments
- Structured log entries with correlation IDs
- Configurable log levels per category

### 7.3 Metrics
Basic metrics are available through:
- .NET runtime metrics
- HTTP request/response metrics
- Custom business metrics (future enhancement)

## 8. Backup and Recovery

### 8.1 Data Persistence
Since the application uses in-memory data stores:
- No persistent data requiring backup
- All content is part of the application deployment
- Updates require new deployments

### 8.2 Recovery Procedures
Recovery is achieved through:
- Restarting the application process
- Redeploying from known good artifacts
- Rolling back to previous versions

## 9. Scaling Considerations

### 9.1 Horizontal Scaling
Multiple instances can be deployed behind a load balancer:
- Statelessness enables easy scaling
- No session affinity required
- Shared static content

### 9.2 Vertical Scaling
Application can utilize additional resources on the same machine:
- Multi-core CPU utilization
- Increased memory allocation
- Enhanced I/O throughput

### 9.3 Limitations
Current architecture has limitations for scaling:
- In-memory data stores prevent data sharing between instances
- No distributed caching implemented
- Static content delivery could benefit from CDN