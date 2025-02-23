using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeautyBeastApi.Endpoints;
public static class ArtistsEndpoints
{
    const string getArtistEndpointName = "GetArtist";
    public static RouteGroupBuilder MapArtistsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("artists").WithParameterValidation();

        // GET /artists
       group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var artists = await dbContext.Artists
                .Include(a => a.Achievements)
                .Include(a => a.Posts)
                    .ThenInclude(p => p.Comments)
                .Include(a => a.Treatments)
                .ToListAsync();

            var artistDtos = artists.Select(a => new ArtistDto(
                a.Id,
                a.FullName,
                a.Email,
                a.ProfilePictureUrl,
                a.DateJoined,
                a.Role, 
                a.Bio,
                a.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                a.Posts.Select(p => new PostDto(
                    p.Id,
                    p.Description,
                    p.MediaUrl,
                    p.DatePosted,
                    p.Comments.Select(c => new CommentDto(
                        c.Id,
                        c.TheComment,
                        c.Likes,
                        c.PostId
                    )).ToList(),
                    p.Likes,
                    p.ArtistId,
                    a.FullName
                )).ToList(),
                a.Treatments.Select(t => new TreatmentDto(
                    t.Id,
                    t.Name,
                    t.Description,
                    t.PreCareInstructions,
                    t.AfterCareInstructions,
                    t.ConsentFormUrl,
                    t.BookingDate,
                    t.StartTime,
                    t.Duration,
                    t.ArtistId,
                    a.FullName
                )).ToList()
            )).ToList();

            return Results.Ok(artistDtos);
        });

        // GET /artists/{id}
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists
                .Include(a => a.Posts)
                    .ThenInclude(p => p.Comments)
                .Include(a => a.Treatments)
                .Include(a => a.Achievements)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return Results.NotFound($"Artist with ID {id} not found.");
            }

            var artistDto = new ArtistDto(
                artist.Id,
                artist.FullName,
                artist.Email,
                artist.ProfilePictureUrl,
                artist.DateJoined,
                artist.Role,
                artist.Bio,
                artist.Achievements?.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList() ?? new List<ArtistAchievementDto>(),
                artist.Posts?.Select(p => new PostDto(
                    p.Id,
                    p.Description,
                    p.MediaUrl,
                    p.DatePosted,
                    p.Comments?.Select(c => new CommentDto(
                        c.Id,
                        c.TheComment,
                        c.Likes,
                        c.PostId
                    )).ToList() ?? new List<CommentDto>(),
                    p.Likes,
                    p.ArtistId,
                    artist.FullName
                )).ToList() ?? new List<PostDto>(),
                artist.Treatments?.Select(t => new TreatmentDto(
                    t.Id,
                    t.Name,
                    t.Description,
                    t.PreCareInstructions,
                    t.AfterCareInstructions,
                    t.ConsentFormUrl,
                    t.BookingDate,
                    t.StartTime,
                    t.Duration,
                    t.ArtistId,
                    artist.FullName
                )).ToList() ?? new List<TreatmentDto>()
            );

            return Results.Ok(artistDto);
        });

        // POST /artists
        group.MapPost("/", async (CreateArtistDto newArtist, BeautyBeastContext dbContext) =>
        {
            if (await dbContext.Artists.AnyAsync(a => a.Email == newArtist.Email))
            {
                return Results.BadRequest("An artist with this email already exists.");
            }

            string hashedPassword = await Task.Run(() => PasswordHelper.HashPassword(newArtist.Password));

            Artist artist = new()
            {
                FullName = newArtist.FullName,
                Email = newArtist.Email,
                ProfilePictureUrl = newArtist.ProfilePictureUrl,
                DateJoined = DateTime.UtcNow,
                HashedPassword = hashedPassword,
                Role = newArtist.Role,
                Bio = newArtist.Bio ?? string.Empty,
                Achievements = newArtist.Achievements?.Select(ach => new ArtistAchievement 
                { 
                    Achievement = ach.Achievement 
                }).ToList() ?? new List<ArtistAchievement>()
            };

            dbContext.Artists.Add(artist);
            await dbContext.SaveChangesAsync();

            var artistDto = new ArtistDto(
                artist.Id,
                artist.FullName,
                artist.Email,
                artist.ProfilePictureUrl,
                artist.DateJoined,
                artist.Role,
                artist.Bio,
                artist.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                new List<PostDto>(),
                new List<TreatmentDto>()
            );

            return Results.Created($"/artists/{artist.Id}", artistDto);
        });

        // PUT /artists/{id}
        group.MapPut("/{id}", async (int id, EditArtistDto editArtist, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists
                .Include(a => a.Achievements)
                .Include(a => a.Posts)
                    .ThenInclude(p => p.Comments)
                .Include(a => a.Treatments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return Results.NotFound($"Artist with ID {id} not found.");
            }

            artist.FullName = !string.IsNullOrEmpty(editArtist.FullName) ? editArtist.FullName : artist.FullName;
            artist.Email = !string.IsNullOrEmpty(editArtist.Email) ? editArtist.Email : artist.Email;
            artist.ProfilePictureUrl = editArtist.ProfilePictureUrl ?? artist.ProfilePictureUrl;
            artist.Bio = editArtist.Bio ?? artist.Bio;

            if (!string.IsNullOrEmpty(editArtist.Role))
            {
                artist.Role = editArtist.Role;
            }

            if (!string.IsNullOrEmpty(editArtist.Password))
            {
                artist.HashedPassword = await Task.Run(() => PasswordHelper.HashPassword(editArtist.Password));
            }

            if (editArtist.Achievements != null)
            {
                var newAchievements = editArtist.Achievements.Select(ach => ach.Achievement).ToHashSet();
                var existingAchievements = artist.Achievements.Select(ach => ach.Achievement).ToHashSet();

                // Add new achievements that are not in existing achievements
                foreach (var ach in newAchievements.Except(existingAchievements))
                {
                    artist.Achievements.Add(new ArtistAchievement { Achievement = ach });
                }

                // Remove achievements that are no longer present in the new list
                artist.Achievements.RemoveAll(ach => !newAchievements.Contains(ach.Achievement));
            }

            await dbContext.SaveChangesAsync();

            // Create DTO for response
            var updatedArtistDto = new ArtistDto(
                artist.Id,
                artist.FullName,
                artist.Email,
                artist.ProfilePictureUrl,
                artist.DateJoined,
                artist.Role, 
                artist.Bio,
                artist.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                artist.Posts.Select(p => new PostDto(
                    p.Id,
                    p.Description,
                    p.MediaUrl,
                    p.DatePosted,
                    p.Comments.Select(c => new CommentDto( 
                        c.Id,
                        c.TheComment,
                        c.Likes,
                        c.PostId
                    )).ToList(),
                    p.Likes,
                    p.ArtistId,
                    artist.FullName
                )).ToList(),
                artist.Treatments.Select(t => new TreatmentDto(
                    t.Id,
                    t.Name,
                    t.Description,
                    t.PreCareInstructions,
                    t.AfterCareInstructions,
                    t.ConsentFormUrl,
                    t.BookingDate,
                    t.StartTime,
                    t.Duration,
                    t.ArtistId,
                    artist.FullName
                )).ToList()
            );

            return Results.Ok(updatedArtistDto);
        });

        // DELETE /artists/{id}
       group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists.FindAsync(id);
            if (artist == null)
            {
                return Results.NotFound($"Artist with ID {id} not found.");
            }

            dbContext.Artists.Remove(artist);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}