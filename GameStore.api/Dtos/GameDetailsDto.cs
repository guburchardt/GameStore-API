namespace GameStore.api.Dtos;

public record GameDetailsDto
(
    int Id,
    String Name, 
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
