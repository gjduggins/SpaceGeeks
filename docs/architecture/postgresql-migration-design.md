# PostgreSQL Migration Design

## 1. Overview

This document describes the architectural changes required to migrate the SpaceGeeks website from in-memory data repositories to PostgreSQL database storage. This migration will enable persistent data storage, better scalability, and support for dynamic content management.

## 2. Requirements

### Functional Requirements
- Replace in-memory data storage with PostgreSQL database storage
- Maintain existing data models and interfaces
- Ensure data persistence across application restarts
- Support CRUD operations for NASA missions and planets data

### Non-Functional Requirements
- Maintain or improve application performance
- Ensure data consistency and integrity
- Provide secure database connections
- Enable horizontal scaling capabilities
- Maintain backward compatibility with existing APIs

## 3. Existing Architecture

### Current Data Layer
The SpaceGeeks application currently uses in-memory repositories for data storage:

1. **InMemoryPlanetRepository** - Stores planet data in a static array
2. **InMemoryNasaMissionRepository** - Stores NASA mission data in a static array

Both repositories implement their respective interfaces:
- `IPlanetRepository`
- `INasaMissionRepository`

### Dependency Injection
Repositories are registered as singleton services in `Program.cs`:
```csharp
builder.Services.AddSingleton<IPlanetRepository, InMemoryPlanetRepository>();
builder.Services.AddSingleton<INasaMissionRepository, InMemoryNasaMissionRepository>();
```

### Data Models
Immutable records are used for data models:
- `Planet` - Represents a planet with properties like name, diameter, mass, etc.
- `NasaMission` - Represents a NASA mission with properties like name, description, launch date, etc.

## 4. Proposed Architecture

### New Data Layer
Replace in-memory repositories with PostgreSQL-based implementations:

1. **PostgreSqlPlanetRepository** - Retrieves planet data from PostgreSQL database
2. **PostgreSqlNasaMissionRepository** - Retrieves NASA mission data from PostgreSQL database

Both repositories will implement the existing interfaces to maintain compatibility:
- `IPlanetRepository`
- `INasaMissionRepository`

### Database Schema
Create PostgreSQL tables to store the data:

#### Planets Table
```sql
CREATE TABLE planets (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    diameter_km DOUBLE PRECISION NOT NULL,
    mass_kg DOUBLE PRECISION NOT NULL,
    distance_from_sun_km DOUBLE PRECISION NOT NULL,
    number_of_moons INTEGER NOT NULL,
    orbital_period_days DOUBLE PRECISION NOT NULL,
    image_path VARCHAR(255) NOT NULL
);
```

#### Nasa Missions Table
```sql
CREATE TABLE nasa_missions (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT NOT NULL,
    launch_date DATE NOT NULL,
    end_date DATE NULL,
    status VARCHAR(50) NOT NULL,
    image_path VARCHAR(255) NOT NULL
);
```

### Dependency Injection Changes
Update service registration in `Program.cs`:
```csharp
builder.Services.AddScoped<IPlanetRepository, PostgreSqlPlanetRepository>();
builder.Services.AddScoped<INasaMissionRepository, PostgreSqlNasaMissionRepository>();
builder.Services.AddScoped<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
```

### Connection Management
Introduce a database connection factory for managing PostgreSQL connections:
- `IDatabaseConnectionFactory` - Interface for creating database connections
- `DatabaseConnectionFactory` - Implementation that creates PostgreSQL connections

## 5. Required Changes

### Existing Components to Modify

#### Program.cs
- MODIFY service registration to use PostgreSQL repositories instead of in-memory ones
- ADD configuration for PostgreSQL connection string
- ADD dependency injection for database connection factory

#### Project Files
- MODIFY `SpaceGeeks.csproj` to add PostgreSQL NuGet package references:
  - `Npgsql` for PostgreSQL connectivity
  - `Dapper` for object-relational mapping (optional but recommended)

### New Components

#### Database Connection Factory
- **Purpose**: Manage PostgreSQL database connections
- **Responsibilities**: Create and dispose database connections
- **Location**: `SpaceGeeks/Data/DatabaseConnectionFactory.cs`
- **Interface**: `IDatabaseConnectionFactory`

#### PostgreSQL Repositories
- **Purpose**: Retrieve data from PostgreSQL database
- **Responsibilities**: Execute SQL queries and map results to domain models
- **Location**: 
  - `SpaceGeeks/Data/PostgreSqlPlanetRepository.cs`
  - `SpaceGeeks/Data/PostgreSqlNasaMissionRepository.cs`

#### Database Initialization Scripts
- **Purpose**: Create database schema and seed initial data
- **Location**: `SpaceGeeks/Scripts/`
- **Files**: 
  - `01_create_tables.sql`
  - `02_seed_data.sql`

### Interfaces and API Changes

#### No Breaking Changes
The existing repository interfaces (`IPlanetRepository`, `INasaMissionRepository`) will remain unchanged, ensuring no modifications are needed in page models or other consumers.

#### New Interface
- **IDatabaseConnectionFactory**: New interface for database connection management

### Data Changes

#### Migration Strategy
1. Create database schema
2. Migrate existing data from in-memory collections to PostgreSQL tables
3. Update application to use PostgreSQL repositories
4. Remove in-memory repository implementations (after verification)

#### Data Models
No changes to existing data models (`Planet`, `NasaMission`) as they will map directly to database tables.

### Configuration and Infrastructure Changes

#### Application Configuration
- ADD PostgreSQL connection string to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "SpaceGeeksDb": "Host=localhost;Database=spacegeeks;Username=spacegeeks;Password=spacegeeks"
  }
}
```

#### Environment Configuration
- UPDATE `appsettings.Development.json` with development database connection
- UPDATE deployment documentation to include PostgreSQL setup instructions

#### Infrastructure Requirements
- ADD PostgreSQL database server (version 13 or later)
- CREATE database user with appropriate permissions
- CONFIGURE network access between application and database

### Security Changes

#### Connection Security
- USE encrypted connections (SSL/TLS) for database communication
- STORE database credentials securely (environment variables/secrets management)
- IMPLEMENT connection pooling for efficient resource usage

#### Data Security
- APPLY principle of least privilege for database user
- USE parameterized queries to prevent SQL injection
- ENCRYPT sensitive data at rest (if any is added in future)

### Observability Changes

#### Logging
- ADD database operation logging
- MONITOR connection pool usage
- TRACK query performance metrics

#### Error Handling
- IMPLEMENT proper error handling for database connectivity issues
- PROVIDE meaningful error messages for database-related failures

### Test Impact

#### Unit Tests
- UPDATE repository tests to work with PostgreSQL repositories
- MOCK database connections where appropriate for unit tests
- ENSURE existing test coverage is maintained

#### Integration Tests
- ADD database integration tests
- TEST database connectivity and query execution
- VERIFY data integrity and consistency

#### Test Data Management
- CREATE test database with sample data
- IMPLEMENT test data seeding strategies
- CLEAN up test data after test execution

### Deployment and Migration Changes

#### Deployment Process
- ADD PostgreSQL database provisioning to deployment pipeline
- INCLUDE database schema creation scripts
- SEED initial data during deployment

#### Migration Process
- DEVELOP data migration scripts to transfer existing data
- EXECUTE migration during deployment
- VALIDATE data integrity post-migration

#### Rollback Considerations
- MAINTAIN in-memory repositories temporarily for rollback capability
- BACKUP database before migration
- DOCUMENT rollback procedures

## 6. Architecture Decisions

### Repository Pattern Continuation
**Decision**: Continue using the repository pattern for data access.
**Rationale**: 
- Maintains existing architectural consistency
- Provides abstraction between business logic and data access
- Enables testability through interface mocking
- Allows for future data source changes without affecting consumers

### Dependency Injection Scope
**Decision**: Register repositories as scoped services rather than singletons.
**Rationale**:
- Better resource management for database connections
- Aligns with typical web application patterns
- Enables proper disposal of database connections
- Improves testability

### ORM Selection
**Decision**: Use Dapper as a lightweight ORM rather than Entity Framework.
**Rationale**:
- Simpler learning curve for existing codebase
- Better performance for straightforward queries
- Less overhead than full ORM frameworks
- Maintains control over SQL queries

### Connection Management
**Decision**: Implement a connection factory pattern for database connections.
**Rationale**:
- Centralizes connection creation logic
- Enables consistent connection configuration
- Facilitates testing through interface mocking
- Allows for future connection pooling optimizations

## 7. Risks and Considerations

### Performance Risks
- Database queries may be slower than in-memory access
- Network latency between application and database
- Mitigation: Implement connection pooling and query optimization

### Availability Risks
- Database downtime affects entire application
- Single point of failure with database server
- Mitigation: Implement database replication and monitoring

### Data Migration Risks
- Potential data loss during migration
- Inconsistent data between old and new systems
- Mitigation: Thorough testing and backup procedures

### Complexity Increase
- Added infrastructure complexity with database management
- More components to monitor and maintain
- Mitigation: Comprehensive documentation and monitoring

## 8. Summary of Required Changes

- MODIFY `SpaceGeeks/Program.cs` to register PostgreSQL repositories and connection factory
- ADD `SpaceGeeks/Data/IDatabaseConnectionFactory.cs` interface
- ADD `SpaceGeeks/Data/DatabaseConnectionFactory.cs` implementation
- ADD `SpaceGeeks/Data/PostgreSqlPlanetRepository.cs` implementation
- ADD `SpaceGeeks/Data/PostgreSqlNasaMissionRepository.cs` implementation
- MODIFY `SpaceGeeks.csproj` to add PostgreSQL and Dapper NuGet packages
- ADD database schema creation scripts
- ADD data seeding scripts
- UPDATE `appsettings.json` with connection string configuration
- UPDATE existing tests to work with new repositories
- ADD new database integration tests
- UPDATE deployment documentation

This migration will significantly improve the application's data persistence capabilities while maintaining its existing architecture and interfaces.