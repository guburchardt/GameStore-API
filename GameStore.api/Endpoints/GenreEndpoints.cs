using GameStore.api.Data;
using GameStore.api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        //GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                            .Select(genre => new GenreDto(genre.Id, genre.Name))
                            .AsNoTracking()
                            .ToListAsync()
        );
    }
}
