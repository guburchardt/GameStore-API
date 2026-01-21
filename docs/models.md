# Data Models

Complete documentation of all data models, DTOs, and their relationships in the GameStore API.

## Entity Models

Entity models represent the database tables and are used internally by Entity Framework Core.

### Game

Represents a game in the store.

**Namespace**: `GameStore.api.Models`

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

**Properties**:
- `Id` (int): Primary key, auto-generated unique identifier
- `Name` (string, required): Game name, maximum 50 characters
- `Genre` (Genre?, nullable): Navigation property to the associated Genre
- `GenreId` (int): Foreign key to the Genres table
- `Price` (decimal): Game price, range 1-150
- `ReleaseDate` (DateOnly): Game release date in YYYY-MM-DD format

**Relationships**:
- Many-to-One with `Genre` via `GenreId`

**Usage**: Used internally by Entity Framework Core for database operations.

---

### Genre

Represents a game genre/category.

**Namespace**: `GameStore.api.Models`

```csharp
public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
```

**Properties**:
- `Id` (int): Primary key, auto-generated unique identifier
- `Name` (string, required): Genre name, must be unique

**Relationships**:
- One-to-Many with `Game` (a genre can have many games)

**Usage**: Used internally by Entity Framework Core for database operations.

**Initial Values**: Seeded with:
- Fighting
- RPG
- Platformer
- Racing
- Sports

---

## Data Transfer Objects (DTOs)

DTOs define the shape of data exchanged between the API and clients. They provide a clean separation between internal models and API contracts.

### Input DTOs

Used for data coming into the API (POST/PUT requests).

#### CreateGameDto

DTO for creating a new game.

**Namespace**: `GameStore.api.Dtos`

```csharp
public record class CreateGameDto
(
    [Required][StringLength(50)] string Name,
    [Range(1,50)] int GenreId,
    [Range(1, 150)]decimal Price,
    DateOnly ReleaseDate
);
```

**Properties**:
- `Name` (string, required): Game name, maximum 50 characters
- `GenreId` (int, required): Genre identifier, range 1-50
- `Price` (decimal, required): Game price, range 1-150
- `ReleaseDate` (DateOnly, required): Release date in YYYY-MM-DD format

**Validation**:
- `Name`: Required, max length 50
- `GenreId`: Required, must be between 1 and 50
- `Price`: Required, must be between 1 and 150
- `ReleaseDate`: Required, must be a valid date

**Usage**: Used in `POST /games` endpoint.

---

#### UpdateGameDto

DTO for updating an existing game.

**Namespace**: `GameStore.api.Dtos`

```csharp
public record UpdateGameDto
(
    [Required][StringLength(50)] string Name,
    [Range(1, 50)] int GenreId,
    [Range(1, 150)]decimal Price,
    DateOnly ReleaseDate
);
```

**Properties**:
- `Name` (string, required): Game name, maximum 50 characters
- `GenreId` (int, required): Genre identifier, range 1-50
- `Price` (decimal, required): Game price, range 1-150
- `ReleaseDate` (DateOnly, required): Release date in YYYY-MM-DD format

**Validation**:
- Same validation rules as `CreateGameDto`

**Usage**: Used in `PUT /games/{id}` endpoint.

---

### Output DTOs

Used for data going out of the API (GET responses).

#### GameSummaryDto

DTO for game list responses, includes genre name.

**Namespace**: `GameStore.api.Dtos`

```csharp
public record GameSummaryDto
(
    int Id,
    String Name, 
    String Genre,
    decimal Price,
    DateOnly ReleaseDate
);
```

**Properties**:
- `Id` (int): Game identifier
- `Name` (string): Game name
- `Genre` (string): Genre name (not ID)
- `Price` (decimal): Game price
- `ReleaseDate` (DateOnly): Release date

**Usage**: Used in `GET /games` endpoint to return a list of games with genre names.

**Note**: This DTO includes the genre name as a string, making it more convenient for clients that don't need to resolve genre IDs.

---

#### GameDetailsDto

DTO for detailed game information, includes genre ID.

**Namespace**: `GameStore.api.Dtos`

```csharp
public record GameDetailsDto
(
    int Id,
    String Name, 
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
```

**Properties**:
- `Id` (int): Game identifier
- `Name` (string): Game name
- `GenreId` (int): Genre identifier
- `Price` (decimal): Game price
- `ReleaseDate` (DateOnly): Release date

**Usage**: 
- Used in `GET /games/{id}` endpoint
- Used in `POST /games` response (after creation)

**Note**: This DTO includes the genre ID, which is useful for update operations.

---

#### GenreDto

DTO for genre information.

**Namespace**: `GameStore.api.Dtos`

```csharp
public record GenreDto(int Id, string Name);
```

**Properties**:
- `Id` (int): Genre identifier
- `Name` (string): Genre name

**Usage**: Used in `GET /genres` endpoint to return a list of all genres.

---

## Model Relationships

```
┌─────────────┐                    ┌─────────────┐
│    Game    │                    │   Genre    │
├─────────────┤                    ├─────────────┤
│ Id (PK)    │                    │ Id (PK)    │
│ Name       │                    │ Name       │
│ GenreId(FK)│───────────────────▶│            │
│ Price      │                    │            │
│ ReleaseDate│                    │            │
└─────────────┘                    └─────────────┘
      │                                   │
      │                                   │
      └─────────── Many-to-One ───────────┘
```

**Relationship Details**:
- One `Genre` can have many `Game` entities
- Each `Game` belongs to exactly one `Genre`
- Foreign key: `Game.GenreId` → `Genre.Id`
- Navigation property: `Game.Genre` provides access to the related `Genre` entity

---

## Data Type Notes

### DateOnly

The API uses `DateOnly` for dates, which:
- Represents only a date (no time component)
- Serializes to ISO 8601 format: `YYYY-MM-DD`
- Example: `2024-01-15`

### Decimal

Prices are stored as `decimal` type:
- Provides precise decimal arithmetic
- Avoids floating-point precision issues
- Range: 1-150 (as per validation)

### Required Properties

Properties marked with `required` keyword:
- Must be provided when creating instances
- Enforced at compile-time (C# 11+)
- Provides better null-safety

---

## Validation Summary

| Property | Type | Constraints | Applies To |
|----------|------|-------------|------------|
| Name | string | Required, Max 50 chars | CreateGameDto, UpdateGameDto |
| GenreId | int | Required, Range 1-50 | CreateGameDto, UpdateGameDto |
| Price | decimal | Required, Range 1-150 | CreateGameDto, UpdateGameDto |
| ReleaseDate | DateOnly | Required, Valid date | CreateGameDto, UpdateGameDto |

---

## Mapping Examples

### Entity to DTO Mapping

**Game to GameSummaryDto**:
```csharp
new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre!.Name,  // Navigation property
    game.Price,
    game.ReleaseDate
)
```

**Game to GameDetailsDto**:
```csharp
new GameDetailsDto(
    game.Id,
    game.Name,
    game.GenreId,  // Foreign key
    game.Price,
    game.ReleaseDate
)
```

### DTO to Entity Mapping

**CreateGameDto to Game**:
```csharp
Game game = new()
{
    Name = newGame.Name,
    GenreId = newGame.GenreId,
    Price = newGame.Price,
    ReleaseDate = newGame.ReleaseDate
};
```

---

## Best Practices

1. **Use DTOs for API boundaries**: Never expose entity models directly in API responses
2. **Validate at the boundary**: All input DTOs have validation attributes
3. **Use records for DTOs**: Immutability and value semantics
4. **Project queries**: Use `Select()` to map directly to DTOs in queries
5. **Use AsNoTracking()**: For read-only queries to improve performance

---

## Future Enhancements

Potential model improvements:
- Add audit fields (CreatedAt, UpdatedAt, CreatedBy)
- Implement soft deletes
- Add more validation attributes
- Create separate DTOs for different use cases
- Add pagination DTOs
- Implement DTO mapping libraries (AutoMapper, Mapster)
