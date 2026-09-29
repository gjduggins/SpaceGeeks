# PostgreSQL Data Access Implementation

## 1. Overview

This document describes the architectural changes required to replace the in-memory data repositories with PostgreSQL database calls in the SpaceGeeks application. The goal is to transition from static, in-memory data storage to a persistent relational database while maintaining the existing application architecture and interfaces.

## 2. Requirements

### Functional Requirements
- Replace `InMemoryNasaMissionRepository` with a PostgreSQL-based implementation
- Replace `InMemoryPlanetRepository` with a PostgreSQL-based implementation
- Maintain the existing repository interfaces (`INasaMissionRepository`, `IPlanetRepository`)
- Ensure data persistence across application restarts
- Support the same data retrieval operations as the current in-memory implementations

### Non-Functional Requirements
- Maintain application performance comparable to in-memory access
- Ensure data consistency and integrity
- Provide appropriate error handling for database operations
- Support secure database connections
- Enable easy deployment and configuration

## 3. Existing Architecture

### Current Data Access Layer
The current implementation uses in-memory repositories that store data as static collections:

1. **InMemoryNasaMissionRepository**
   - Stores NASA mission data in a static array
   - Implements `INasaMissionRepository` interface
   - Provides `GetAllOrderedByLaunchDate()` method
   - Data is initialized at class load time

2. **InMemoryPlanetRepository**
   - Stores planet data in a static read-only list
   - Implements `IPlanetRepository` interface
   - Provides `GetAllOrderedByDistance()` method
   - Data is initialized at class load time

### Dependency Injection
Repositories are registered as singleton services in `Program.cs`:
```csharp
builder.Services.AddSingleton<IPlanetRepository, InMemoryPlanetRepository>();
builder.Services.AddSingleton<INasaMissionRepository, InMemoryNasaMissionRepository>();
```

### Data Models
Both `NasaMission` and `Planet` are implemented as immutable C# records with all properties set at construction time.

## 4. Proposed Architecture

### New Data Access Layer
Replace in-memory repositories with PostgreSQL-based implementations:

1. **PostgreSqlNasaMissionRepository**
   - Implements `INasaMissionRepository` interface
   - Connects to PostgreSQL database
   - Retrieves NASA mission data from database tables
   - Maintains same method signatures as in-memory implementation

2. **PostgreSqlPlanetRepository**
   - Implements `IPlanetRepository` interface
   - Connects to PostgreSQL database
   - Retrieves planet data from database tables
   - Maintains same method signatures as in-memory implementation

### Database Schema
Two tables will be created to match the existing data models:

#### NasaMissions Table
```sql
CREATE TABLE NasaMissions (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    LaunchDate DATE NOT NULL,
    EndDate DATE NULL,
    Status VARCHAR(50) NOT NULL,
    ImagePath VARCHAR(255) NOT NULL
);
```

#### Planets Table
```sql
CREATE TABLE Planets (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    DiameterKm DOUBLE PRECISION NOT NULL,
    MassKg DOUBLE PRECISION NOT NULL,
    DistanceFromSunKm DOUBLE PRECISION NOT NULL,
    NumberOfMoons INTEGER NOT NULL,
    OrbitalPeriodDays DOUBLE PRECISION NOT NULL,
    ImagePath VARCHAR(255) NOT NULL
);
```

### Dependency Injection Updates
Update service registration in `Program.cs`:
```csharp
builder.Services.AddScoped<IPlanetRepository, PostgreSqlPlanetRepository>();
builder.Services.AddScoped<INasaMissionRepository, PostgreSqlNasaMissionRepository>();
```

Note: Using scoped lifetime instead of singleton to ensure proper database connection management.

### Connection Management
- Use connection strings from configuration
- Implement proper connection disposal
- Use parameterized queries to prevent SQL injection

## 5. Required Changes

### 5.1 New Components

#### PostgreSqlNasaMissionRepository (NEW)
- **Purpose**: Retrieve NASA mission data from PostgreSQL database
- **Location**: `SpaceGeeks.Data.PostgreSqlNasaMissionRepository`
- **Dependencies**: PostgreSQL database, Npgsql library
- **Interface**: Implements `INasaMissionRepository`

#### PostgreSqlPlanetRepository (NEW)
- **Purpose**: Retrieve planet data from PostgreSQL database
- **Location**: `SpaceGeeks.Data.PostgreSqlPlanetRepository`
- **Dependencies**: PostgreSQL database, Npgsql library
- **Interface**: Implements `IPlanetRepository`

### 5.2 Existing Components to Modify

#### Program.cs (MODIFY)
- Update dependency injection registrations to use PostgreSQL implementations
- Add configuration for database connection strings
- Add required NuGet package references

#### SpaceGeeks.csproj (MODIFY)
- Add reference to Npgsql NuGet package for PostgreSQL connectivity

#### appsettings.json (MODIFY)
- Add database connection string configuration

### 5.3 Data Changes

#### Database Initialization (ADD)
- Create SQL scripts to initialize database schema
- Create seed data scripts to populate initial data
- Document database setup process

### 5.4 Configuration Changes

#### Application Configuration (CONFIGURE)
- Add connection string to `appsettings.json`
- Add environment-specific configuration support
- Secure sensitive configuration values

### 5.5 Security Changes

#### Database Security (ADD)
- Implement secure connection strings
- Use parameterized queries to prevent SQL injection
- Follow principle of least privilege for database access

### 5.6 Test Impact

#### Unit Tests (MODIFY)
- Update repository tests to work with database implementations
- Add integration tests for database connectivity
- Consider test database setup for isolated testing

#### Test Configuration (ADD)
- Add test-specific connection strings
- Implement test data cleanup strategies

### 5.7 Deployment Changes

#### Infrastructure (ADD)
- PostgreSQL database server requirement
- Database initialization process
- Connection string configuration in deployment environments

## 6. Implementation Approach

### Phase 1: Foundation
1. Add PostgreSQL NuGet package dependency
2. Create database schema and initialization scripts
3. Set up connection string configuration

### Phase 2: Repository Implementation
1. Implement `PostgreSqlNasaMissionRepository`
2. Implement `PostgreSqlPlanetRepository`
3. Ensure implementations match existing interface contracts

### Phase 3: Integration and Testing
1. Update dependency injection configuration
2. Test database connectivity and data retrieval
3. Validate data integrity and ordering
4. Update unit tests for new implementations

### Phase 4: Deployment
1. Deploy database schema to target environments
2. Configure connection strings in deployment environments
3. Verify application functionality with database backend

## 7. Architecture Decisions

### Repository Pattern Continuation
**Decision**: Continue using the repository pattern
**Rationale**: Maintains existing architectural consistency and provides clean separation of concerns

### Dependency Injection Lifetime
**Decision**: Use scoped lifetime for repository implementations
**Rationale**: Properly manages database connection lifecycle and follows ASP.NET Core best practices

### Interface Compatibility
**Decision**: Maintain exact interface compatibility with existing implementations
**Rationale**: Minimizes impact on dependent code and enables smooth transition

## 8. Risks and Considerations

### Performance
- Database queries may be slower than in-memory access
- Consider caching strategies for frequently accessed data
- Monitor query performance and optimize as needed

### Deployment Complexity
- Adds database server dependency
- Requires database initialization and migration processes
- Need for connection string management in different environments

### Data Consistency
- Need to ensure data seeding matches current in-memory data
- Consider backup and recovery strategies
- Handle database connection failures gracefully

### Testing
- Need test database instances for automated testing
- May require different test strategies for integration tests
- Consider data isolation between test runs

## 9. Summary of Required Changes

- ADD `PostgreSqlNasaMissionRepository` implementation
- ADD `PostgreSqlPlanetRepository` implementation
- MODIFY `Program.cs` to register new repository implementations
- MODIFY `SpaceGeeks.csproj` to add Npgsql NuGet package reference
- MODIFY `appsettings.json` to add database connection string
- ADD database schema creation scripts
- ADD database seed data scripts
- CONFIGURE connection strings for different environments
- MODIFY unit tests to work with database implementations
- ADD integration tests for database connectivity