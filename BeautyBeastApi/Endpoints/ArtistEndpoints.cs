using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos.ArtistAchievementsDtos;
using BeautyBeastApi.Dtos.CommentDtos;
using BeautyBeastApi.Dtos.PostDtos;
using BeautyBeastApi.Dtos.TreatmentsDtos;
using BeautyBeastApi.Dtos.UserDtos;
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
                .ToListAsync();

            var posts = await dbContext.Posts.ToListAsync();
            var comments = await dbContext.Comments.ToListAsync();
            var treatments = await dbContext.Treatments.ToListAsync();

            var artistDtos = artists.Select(a => new ArtistDto(
                a.Id,
                a.FullName,
                a.Email,
                a.ProfilePictureUrl,
                a.DateJoined,
                a.Bio,
                a.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                posts.Where(p => p.ArtistId == a.Id).Select(p => new PostDto(
                    p.Id,
                    p.Description,
                    p.MediaUrls,
                    p.DatePosted,
                    comments.Where(c => c.PostId == p.Id).Select(c => new CommentDto(
                        c.Id,
                        c.TheComment,
                        c.Likes,
                        c.PostId
                    )).ToList(),
                    p.Likes,
                    p.ArtistId,
                    a.FullName
                )).ToList(),
                treatments.Where(t => t.ArtistId == a.Id).Select(t => new TreatmentDto(
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
                .Include(a => a.Treatments)
                .Include(a => a.Achievements)
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
                artist.Bio,
                artist.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                artist.Posts.Select(p => new PostDto(
                    p.Id,
                    p.Description,
                    p.MediaUrls,
                    p.DatePosted,
                    p.Comments.Select(c => new CommentDto(c.Id, c.TheComment, c.Likes, c.PostId)).ToList(),
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

            return Results.Ok(artistDto);
        });

        // POST /artists
        group.MapPost("/", async (CreateArtistDto newArtist, BeautyBeastContext dbContext) =>
        {
            DateTime dateJoined = DateTime.UtcNow;

            Artist artist = new()
            {
                FullName = newArtist.FullName,
                Email = newArtist.Email,
                ProfilePictureUrl = newArtist.ProfilePictureUrl,
                DateJoined = dateJoined,
                Bio = newArtist.Bio,
                Achievements = newArtist.Achievements.Select(ach => new ArtistAchievement { Achievement = ach.Achievement }).ToList()
            };

            dbContext.Artists.Add(artist);
            await dbContext.SaveChangesAsync();

            var artistDto = new ArtistDto(
                artist.Id,
                artist.FullName,
                artist.Email,
                artist.ProfilePictureUrl,
                artist.DateJoined,
                artist.Bio,
                artist.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                new List<PostDto>(), // empty initially
                new List<TreatmentDto>() // emmpty initially
            );
            
            return Results.Created($"/artists/{artist.Id}", artistDto);
        });

        // PUT /artists/{id}
        group.MapPut("/{id}", async (int id, EditArtistDto editArtist, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists
                .Include(a => a.Posts)
                .ThenInclude(p => p.Comments)
                .Include(a => a.Treatments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
            {
                return Results.NotFound($"Artist with ID {id} not found.");
            }

            // Preserve old values if new ones are not provided
            artist.FullName = editArtist.FullName ?? artist.FullName;
            artist.Email = editArtist.Email ?? artist.Email;
            artist.ProfilePictureUrl = editArtist.ProfilePictureUrl ?? artist.ProfilePictureUrl;
            artist.Bio = editArtist.Bio ?? artist.Bio;
            artist.Achievements = editArtist.Achievements != null
                ? editArtist.Achievements.Select(ach => new ArtistAchievement { Achievement = ach.Achievement }).ToList()
                : artist.Achievements;

            await dbContext.SaveChangesAsync();

            var updatedArtistDto = new ArtistDto(
                artist.Id,
                artist.FullName,
                artist.Email,
                artist.ProfilePictureUrl,
                artist.DateJoined,
                artist.Bio,
                artist.Achievements.Select(ach => new ArtistAchievementDto(ach.Id, ach.Achievement)).ToList(),
                artist.Posts.Select(p => new PostDto(
                    p.Id,
                    p.Description,
                    p.MediaUrls,
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