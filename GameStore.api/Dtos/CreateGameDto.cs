using System.ComponentModel.DataAnnotations;

namespace GameStore.api.Dtos;

public record class CreateGameDto
(
    [Required][StringLength(50)] string Name,
    [Range(1,50)] int GenreId,
    [Range(1, 150)]decimal Price,
    DateOnly ReleaseDate
);
