# API Reference

Complete documentation for all available endpoints in the GameStore API.

## Base URL

- **Development HTTP**: `http://localhost:5058`
- **Development HTTPS**: `https://localhost:7023`

## Games Endpoints

All game endpoints are prefixed with `/games`.

### Get All Games

Retrieves a list of all games with summary information.

**Endpoint**: `GET /games`

**Response**: `200 OK`

```json
[
  {
    "id": 1,
    "name": "Super Game",
    "genre": "RPG",
    "price": 59.99,
    "releaseDate": "2024-01-15"
  },
  {
    "id": 2,
    "name": "Racing Adventure",
    "genre": "Racing",
    "price": 49.99,
    "releaseDate": "2024-02-20"
  }
]
```

**Response Model**: `GameSummaryDto`
- `id` (int): Unique game identifier
- `name` (string): Game name
- `genre` (string): Genre name
- `price` (decimal): Game price
- `releaseDate` (DateOnly): Release date in YYYY-MM-DD format

---

### Get Game by ID

Retrieves detailed information about a specific game.

**Endpoint**: `GET /games/{id}`

**Parameters**:
- `id` (int, path): Game identifier

**Response**: `200 OK`

```json
{
  "id": 1,
  "name": "Super Game",
  "genreId": 2,
  "price": 59.99,
  "releaseDate": "2024-01-15"
}
```

**Response Model**: `GameDetailsDto`
- `id` (int): Unique game identifier
- `name` (string): Game name
- `genreId` (int): Genre identifier
- `price` (decimal): Game price
- `releaseDate` (DateOnly): Release date in YYYY-MM-DD format

**Error Responses**:
- `404 Not Found`: Game with the specified ID does not exist

---

### Create Game

Creates a new game in the store.

**Endpoint**: `POST /games`

**Request Body**: `CreateGameDto`

```json
{
  "name": "New Game Title",
  "genreId": 2,
  "price": 39.99,
  "releaseDate": "2024-03-01"
}
```

**Request Model**: `CreateGameDto`
- `name` (string, required): Game name (max 50 characters)
- `genreId` (int, required): Genre identifier (range: 1-50)
- `price` (decimal, required): Game price (range: 1-150)
- `releaseDate` (DateOnly, required): Release date in YYYY-MM-DD format

**Response**: `201 Created`

```json
{
  "id": 3,
  "name": "New Game Title",
  "genreId": 2,
  "price": 39.99,
  "releaseDate": "2024-03-01"
}
```

**Response Headers**:
- `Location`: URL to the created resource (e.g., `/games/3`)

**Error Responses**:
- `400 Bad Request`: Validation errors in request body

---

### Update Game

Updates an existing game.

**Endpoint**: `PUT /games/{id}`

**Parameters**:
- `id` (int, path): Game identifier

**Request Body**: `UpdateGameDto`

```json
{
  "name": "Updated Game Title",
  "genreId": 3,
  "price": 49.99,
  "releaseDate": "2024-04-01"
}
```

**Request Model**: `UpdateGameDto`
- `name` (string, required): Game name (max 50 characters)
- `genreId` (int, required): Genre identifier (range: 1-50)
- `price` (decimal, required): Game price (range: 1-150)
- `releaseDate` (DateOnly, required): Release date in YYYY-MM-DD format

**Response**: `204 No Content`

**Error Responses**:
- `400 Bad Request`: Validation errors in request body
- `404 Not Found`: Game with the specified ID does not exist

---

### Delete Game

Deletes a game from the store.

**Endpoint**: `DELETE /games/{id}`

**Parameters**:
- `id` (int, path): Game identifier

**Response**: `204 No Content`

**Error Responses**:
- `404 Not Found`: Game with the specified ID does not exist (though the endpoint returns 204 regardless)

---

## Genres Endpoints

All genre endpoints are prefixed with `/genres`.

### Get All Genres

Retrieves a list of all available genres.

**Endpoint**: `GET /genres`

**Response**: `200 OK`

```json
[
  {
    "id": 1,
    "name": "Fighting"
  },
  {
    "id": 2,
    "name": "RPG"
  },
  {
    "id": 3,
    "name": "Platformer"
  },
  {
    "id": 4,
    "name": "Racing"
  },
  {
    "id": 5,
    "name": "Sports"
  }
]
```

**Response Model**: `GenreDto`
- `id` (int): Unique genre identifier
- `name` (string): Genre name

**Note**: Genres are automatically seeded when the database is first created. The initial genres are:
- Fighting
- RPG
- Platformer
- Racing
- Sports

---

## Validation Rules

### Game Name
- **Required**: Yes
- **Max Length**: 50 characters
- **Type**: String

### Genre ID
- **Required**: Yes
- **Range**: 1-50
- **Type**: Integer
- **Note**: Must reference an existing genre

### Price
- **Required**: Yes
- **Range**: 1-150
- **Type**: Decimal

### Release Date
- **Required**: Yes
- **Format**: YYYY-MM-DD (ISO 8601 date format)
- **Type**: DateOnly

---

## Error Handling

The API uses standard HTTP status codes:

- `200 OK`: Successful GET request
- `201 Created`: Successful POST request
- `204 No Content`: Successful PUT/DELETE request
- `400 Bad Request`: Validation errors or malformed request
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

---

## Example Requests

### Using curl

```bash
# Get all games
curl http://localhost:5058/games

# Get game by ID
curl http://localhost:5058/games/1

# Create a new game
curl -X POST http://localhost:5058/games \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Epic Adventure",
    "genreId": 2,
    "price": 59.99,
    "releaseDate": "2024-06-15"
  }'

# Update a game
curl -X PUT http://localhost:5058/games/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Epic Adventure",
    "genreId": 2,
    "price": 49.99,
    "releaseDate": "2024-06-15"
  }'

# Delete a game
curl -X DELETE http://localhost:5058/games/1

# Get all genres
curl http://localhost:5058/genres
```

### Using HTTPie

```bash
# Get all games
http GET localhost:5058/games

# Create a new game
http POST localhost:5058/games \
  name="Epic Adventure" \
  genreId:=2 \
  price:=59.99 \
  releaseDate="2024-06-15"
```

---

## Notes

- All dates must be in ISO 8601 format (YYYY-MM-DD)
- Price values are in decimal format
- The API does not currently support pagination or filtering
- Genre relationships are validated implicitly (no explicit foreign key validation in API layer)
