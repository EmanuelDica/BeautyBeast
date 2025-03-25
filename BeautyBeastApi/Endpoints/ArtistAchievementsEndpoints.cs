using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BeautyBeastApi.Endpoints;

public static class ArtistAchievementsEndpoints
{
    const string getByArtistIdEndpointName = "GetAchievementsByArtistId";

    public static RouteGroupBuilder MapArtistAchievementsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("artist-achievements").WithParameterValidation();

        //GET /artist-achievements - Get all achievements
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var allAchievements = await dbContext.ArtistAchievements
                .Select(a => new ArtistAchievementDto(a.Id, a.Achievement))
                .ToListAsync();

            return Results.Ok(allAchievements);
        });

        //GET /artist-achievements/{artistId} - Get achievements by artistId
        group.MapGet("/{artistId}", async (int artistId, BeautyBeastContext dbContext) =>
        {
            var achievements = await dbContext.ArtistAchievements
                .Where(a => a.ArtistId == artistId)
                .Select(a => new ArtistAchievementDto(a.Id, a.Achievement))
                .ToListAsync();

                if (achievements == null || achievements.Count == 0)
                    return Results.NotFound($"No achievements found for artist with ID {artistId}.");

            return Results.Ok(achievements);
        }).WithName(getByArtistIdEndpointName);

        //POST /artist-achievements/{artistId} - Add new achievement for artist
        group.MapPost("/{artistId}", async (int artistId, CreateArtistAchievementDto newAchievement, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists.FindAsync(artistId);
            if (artist == null)
                return Results.NotFound($"Artist with ID {artistId} not found.");

            var achievement = new ArtistAchievement
            {
                Achievement = newAchievement.Achievement,
                ArtistId = artistId
            };

            dbContext.ArtistAchievements.Add(achievement);
            await dbContext.SaveChangesAsync();

            var achievementDto = new ArtistAchievementDto(achievement.Id, achievement.Achievement);
            return Results.Created($"/artist-achievements/{artistId}", achievementDto); // Created response with location header
        });

        //PUT /artist-achievements/{artistId} - Replace all achievements for artist
        group.MapPut("/{artistId}", async (int artistId, EditUserAchievementsDto editAchievements, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists
                .Include(a => a.Achievements)
                .FirstOrDefaultAsync(a => a.Id == artistId);

            if (artist == null)
                return Results.NotFound($"Artist with ID {artistId} not found.");

            dbContext.ArtistAchievements.RemoveRange(artist.Achievements);

            var newAchievements = editAchievements.Achievements.Select(a => new ArtistAchievement
            {
                Achievement = a,
                ArtistId = artistId
            }).ToList();

            dbContext.ArtistAchievements.AddRange(newAchievements);
            await dbContext.SaveChangesAsync();

            var newAchievementDtos = newAchievements
                .Select(a => new ArtistAchievementDto(a.Id, a.Achievement))
                .ToList();

            return Results.Ok(newAchievementDtos);
        });

        //DELETE /artist-achievements/{id} - Delete specific achievement by Id
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var achievement = await dbContext.ArtistAchievements.FindAsync(id);
            if (achievement == null)
                return Results.NotFound($"Achievement with ID {id} not found.");

            dbContext.ArtistAchievements.Remove(achievement);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}