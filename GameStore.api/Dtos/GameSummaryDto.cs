namespace GameStore.api.Dtos;

public record GameSummaryDto
(
    int Id,
    String Name, 
    String Genre,
    decimal Price,
    DateOnly ReleaseDate
);
