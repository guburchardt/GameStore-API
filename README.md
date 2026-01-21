# GameStore API

A RESTful API for managing a game store built with .NET 10.0, featuring game and genre management with Entity Framework Core and SQLite.

## Quick Start

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- A code editor (Visual Studio, VS Code, Rider, etc.)

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd netCourse
   ```

2. **Navigate to the API project**
   ```bash
   cd GameStore.api
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - HTTP: `http://localhost:5058`
   - HTTPS: `https://localhost:7023`

The database will be automatically created and seeded with initial genre data on first run.

### Testing the API

You can use the included `games.http` file with REST Client extensions, or use tools like:
- Postman
- curl
- HTTPie
- Swagger UI (if configured)

## Documentation

- [Architecture](./docs/architecture.md) - System architecture and design patterns
- [API Reference](./docs/api.md) - Complete API endpoint documentation
- [Database Schema](./docs/database.md) - Database structure and migrations
- [Data Models](./docs/models.md) - Entity models and DTOs

## Technology Stack

- **.NET 10.0** - Framework
- **Entity Framework Core 10.0** - ORM
- **SQLite** - Database
- **Minimal APIs** - API endpoints
- **FluentValidation** - Input validation

## Project Structure

```
GameStore.api/
├── Data/              # Database context and extensions
├── Dtos/              # Data Transfer Objects
├── Endpoints/         # API endpoint definitions
├── Models/            # Entity models
├── Migrations/        # EF Core migrations
└── Properties/        # Configuration files
```
