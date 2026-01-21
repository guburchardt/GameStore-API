# Architecture

This document describes the overall architecture and design decisions of the GameStore API.

## Overview

The GameStore API is built using .NET 10.0 with a clean, modular architecture following best practices for minimal APIs and Entity Framework Core.

## Architecture Pattern

The application follows a **layered architecture** pattern with clear separation of concerns:

```
┌─────────────────────────────────┐
│      API Endpoints Layer         │  (Endpoints/)
├─────────────────────────────────┤
│      Business Logic Layer       │  (Dtos, Validation)
├─────────────────────────────────┤
│      Data Access Layer          │  (Data/, Models/)
├─────────────────────────────────┤
│      Database                   │  (SQLite)
└─────────────────────────────────┘
```

## Key Components

### 1. Endpoints Layer (`Endpoints/`)

- **Purpose**: Defines HTTP endpoints using .NET Minimal APIs
- **Responsibilities**:
  - Route mapping
  - Request/response handling
  - HTTP status code management
- **Files**:
  - `GamesEndpoints.cs` - Game CRUD operations
  - `GenreEndpoints.cs` - Genre listing operations

### 2. Data Transfer Objects (`Dtos/`)

- **Purpose**: Define the shape of data exchanged between client and server
- **Types**:
  - **Input DTOs**: `CreateGameDto`, `UpdateGameDto` - Used for POST/PUT requests
  - **Output DTOs**: `GameSummaryDto`, `GameDetailsDto`, `GenreDto` - Used for GET responses
- **Benefits**:
  - Decouples internal models from API contracts
  - Enables validation attributes
  - Provides type safety

### 3. Data Access Layer (`Data/`)

- **GameStoreContext**: Entity Framework Core DbContext
  - Manages database connections
  - Provides LINQ queries
  - Handles change tracking

- **DataExtensions**: Extension methods for:
  - Database configuration (`AddGameStoreDb`)
  - Automatic migrations (`MigrateDb`)
  - Database seeding (initial genre data)

### 4. Models (`Models/`)

- **Purpose**: Entity models representing database tables
- **Entities**:
  - `Game` - Represents a game in the store
  - `Genre` - Represents a game genre/category
- **Relationships**:
  - Game has a many-to-one relationship with Genre

## Design Patterns

### 1. Extension Methods Pattern

Used extensively to keep `Program.cs` clean and modular:
- `MapGamesEndpoints()` - Extends `WebApplication`
- `MapGenresEndpoints()` - Extends `WebApplication`
- `AddGameStoreDb()` - Extends `WebApplicationBuilder`
- `MigrateDb()` - Extends `WebApplication`

### 2. Repository Pattern (Implicit)

While not explicitly implemented, the `GameStoreContext` acts as a repository, providing data access abstraction.

### 3. DTO Pattern

Strict separation between domain models and API contracts using DTOs ensures:
- API versioning flexibility
- Data transformation
- Validation at the boundary

## Data Flow

```
Client Request
    ↓
Endpoint (GamesEndpoints/GenreEndpoints)
    ↓
DTO Validation
    ↓
GameStoreContext (EF Core)
    ↓
SQLite Database
    ↓
Response DTO
    ↓
Client Response
```

## Configuration

### Application Configuration

- **appsettings.json**: Production configuration
- **appsettings.Development.json**: Development-specific settings
- **Connection String**: Configured in `appsettings.json` pointing to SQLite database

### Database Initialization

The application uses **automatic migrations** with seeding:
- Migrations run automatically on startup via `MigrateDb()`
- Initial genres are seeded if the database is empty
- No manual migration commands required for basic setup

## Security Considerations

- Input validation using Data Annotations
- SQL injection protection via EF Core parameterized queries
- No authentication/authorization implemented (portfolio project)

## Scalability Notes

- Current architecture is suitable for small to medium applications
- For larger scale, consider:
  - Adding a service layer for business logic
  - Implementing caching
  - Adding authentication/authorization
  - Using a more robust database (PostgreSQL, SQL Server)

## Future Enhancements

Potential improvements:
- Service layer for business logic
- Repository pattern implementation
- Caching layer
- Authentication and authorization
- API versioning
- Swagger/OpenAPI documentation
- Unit and integration tests
