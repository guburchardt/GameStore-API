# Database Schema

Documentation for the database structure, migrations, and data management in the GameStore API.

## Database Technology

- **Database Engine**: SQLite
- **ORM**: Entity Framework Core 10.0
- **Database File**: `GameStore.db` (created automatically)

## Schema Overview

The database consists of two main tables:

```
┌─────────────┐         ┌─────────────┐
│    Games    │────────▶│   Genres    │
└─────────────┘         └─────────────┘
     │                        │
     │                        │
  Many-to-One relationship
```

## Tables

### Games Table

Stores information about games in the store.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | INTEGER | PRIMARY KEY, AUTOINCREMENT | Unique game identifier |
| `Name` | TEXT | NOT NULL, Required | Game name (max 50 characters) |
| `GenreId` | INTEGER | NOT NULL, FOREIGN KEY | Reference to Genres table |
| `Price` | REAL | NOT NULL | Game price (decimal) |
| `ReleaseDate` | TEXT | NOT NULL | Release date (stored as ISO 8601 string) |

**Indexes**: 
- Primary key on `Id`
- Foreign key index on `GenreId`

**Relationships**:
- Many-to-One with `Genres` table via `GenreId`

### Genres Table

Stores game genre categories.

| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| `Id` | INTEGER | PRIMARY KEY, AUTOINCREMENT | Unique genre identifier |
| `Name` | TEXT | NOT NULL, Required, UNIQUE | Genre name |

**Indexes**: 
- Primary key on `Id`
- Unique constraint on `Name`

**Relationships**:
- One-to-Many with `Games` table

## Entity Framework Models

### Game Model

```csharp
public class Game
{
    public int Id { get; set; }
    public required String Name { get; set; }
    public Genre? Genre { get; set; }
    public int GenreId { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
}
```

### Genre Model

```csharp
public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
```

## Migrations

### Initial Migration

**Migration Name**: `20260115220705_InitialCreate`

This migration creates:
- `Games` table with all columns and foreign key relationship
- `Genres` table with all columns
- Primary keys and indexes
- Foreign key constraint from `Games.GenreId` to `Genres.Id`

### Automatic Migrations

The application automatically applies migrations on startup using the `MigrateDb()` extension method:

```csharp
app.MigrateDb();
```

This ensures the database schema is always up-to-date without manual intervention.

## Database Seeding

### Initial Data

The database is automatically seeded with initial genre data when first created:

```csharp
new Genre {Name = "Fighting"},
new Genre {Name = "RPG"},
new Genre {Name = "Platformer"},
new Genre {Name = "Racing"},
new Genre {Name = "Sports"}
```

**Seeding Logic**:
- Seeding occurs during database initialization
- Only runs if the `Genres` table is empty
- Prevents duplicate seeding on subsequent application starts

### Seeding Implementation

Seeding is configured in `DataExtensions.cs` using EF Core's `UseSeeding` method:

```csharp
optionsAction: options => options.UseSeeding((context, _) =>
{
    if (!context.Set<Genre>().Any())
    {
        context.Set<Genre>().AddRange(/* genres */);
        context.SaveChanges();
    }
})
```

## Connection String

The database connection is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "GameStore": "Data Source=GameStore.db"
  }
}
```

This creates a SQLite database file named `GameStore.db` in the application's root directory.

## Database Context

### GameStoreContext

The `GameStoreContext` class manages database operations:

```csharp
public class GameStoreContext : DbContext
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
}
```

**Configuration**:
- Registered as a scoped service
- Uses SQLite provider
- Configured with automatic seeding

## Data Access Patterns

### Querying

The application uses Entity Framework Core LINQ queries:

```csharp
// Get all games with genre information
await dbContext.Games
    .Include(game => game.Genre)
    .Select(game => new GameSummaryDto(...))
    .AsNoTracking()
    .ToListAsync();
```

### Change Tracking

- **Tracking**: Used for updates and deletes
- **No Tracking**: Used for read-only queries via `AsNoTracking()`

### Transactions

Entity Framework Core automatically manages transactions for `SaveChangesAsync()` operations.

## Database File Management

### Location

The `GameStore.db` file is created in the `GameStore.api` project directory.

### Backup and Restore

To backup the database:
```bash
cp GameStore.api/GameStore.db GameStore.api/GameStore.db.backup
```

To restore:
```bash
cp GameStore.api/GameStore.db.backup GameStore.api/GameStore.db
```

### Resetting the Database

To reset the database:
1. Delete `GameStore.db` and `GameStore.db-shm`, `GameStore.db-wal` files
2. Restart the application
3. Migrations will recreate the schema and seed initial data

## Performance Considerations

### Indexes

- Primary keys are automatically indexed
- Foreign keys may benefit from explicit indexes for large datasets

### Query Optimization

- Use `AsNoTracking()` for read-only queries
- Use `Include()` to eagerly load related entities
- Consider projection (Select) to limit data transfer

### SQLite Limitations

- SQLite is suitable for development and small to medium applications
- For production at scale, consider migrating to PostgreSQL or SQL Server
- SQLite has limitations on concurrent writes

## Migration Commands

While migrations run automatically, you can also manage them manually:

```bash
# Create a new migration
dotnet ef migrations add MigrationName --project GameStore.api

# Apply migrations
dotnet ef database update --project GameStore.api

# Remove last migration
dotnet ef migrations remove --project GameStore.api
```

## Future Enhancements

Potential database improvements:
- Add indexes on frequently queried columns
- Implement soft deletes
- Add audit fields (CreatedAt, UpdatedAt)
- Add database-level constraints
- Consider database views for complex queries
- Implement database-level validation
