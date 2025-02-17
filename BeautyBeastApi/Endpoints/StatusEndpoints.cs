using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos.StatusDtos;
using BeautyBeastApi.Dtos.CommentDtos;
using BeautyBeastApi.Dtos.UserDtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;
namespace BeautyBeastApi.Endpoints;

public static class StatusEndpoints
{
    const string getStatusEndpointName = "GetStatus";
    public static RouteGroupBuilder MapStatusEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("statuses").WithParameterValidation();

        // GET /statuses
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
{
    var statuses = await dbContext.Statuses
        .Include(s => s.User)
        .Include(s => s.Comments)
        .Select(s => new StatusDto(
            s.Id,
            s.Text,
            s.DatePosted,
            s.Comments != null
                ? s.Comments.Select(c => new CommentDto(
                    c.Id,
                    c.TheComment,
                    c.Likes,
                    c.PostId
                )).ToList()
                : new List<CommentDto>(), // ensures it's never null
            s.Likes,
            s.UserId,
            s.User != null ? s.User.FullName : "Unknown User" //null user
        ))
        .ToListAsync();

    return Results.Ok(statuses);
});

        // GET /statuses/{id}
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var status = await dbContext.Statuses
                .Where(s => s.Id == id)
                .Include(s => s.User)
                .Include(s => s.Comments)
                .Select(s => new StatusDto(
                    s.Id,
                    s.Text,
                    s.DatePosted,
                    s.Comments != null
                        ? s.Comments.Select(c => new CommentDto(
                            c.Id,
                            c.TheComment,
                            c.Likes,
                            c.PostId
                        )).ToList()
                        : new List<CommentDto>(), // ensures it's never null
                    s.Likes,
                    s.UserId,
                    s.User != null ? s.User.FullName : "Unknown User" // handles null user
                ))
                .FirstOrDefaultAsync();

            return status is null ? Results.NotFound($"Status with ID {id} not found.") : Results.Ok(status);
        })
        .WithName(getStatusEndpointName);
        
        // POST /statuses
        group.MapPost("/", async (CreateStatusDto newStatus, BeautyBeastContext dbContext) =>
        {
            var user = await dbContext.Users.FindAsync(newStatus.UserId);
            if (user == null)
            {
                return Results.BadRequest($"User with ID {newStatus.UserId} not found.");
            }

            Status status = new()
            {
                Text = newStatus.Text,
                DatePosted = DateTime.UtcNow, //timestamp
                Likes = 0, //zero likes initially
                UserId = newStatus.UserId
            };

            dbContext.Statuses.Add(status);
            await dbContext.SaveChangesAsync();

            var statusDto = new StatusDto(
                status.Id,
                status.Text,
                status.DatePosted,
                new List<CommentDto>(), //empty list when initialised
                status.Likes,
                status.UserId,
                user.FullName
            );

            return Results.CreatedAtRoute(getStatusEndpointName, new { id = status.Id }, statusDto);
        });

        // PUT /statuses/{id}
        group.MapPut("/{id}", async (int id, EditStatusDto editStatus, BeautyBeastContext dbContext) =>
        {
            var status = await dbContext.Statuses
                .Include(s => s.User)
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (status == null)
            {
                return Results.NotFound($"Status with ID {id} not found.");
            }
            // keep the old values if new ones not provided
            status.Text = editStatus.Text ?? status.Text;

            await dbContext.SaveChangesAsync();

            var updatedStatusDto = new StatusDto(
                status.Id,
                status.Text,
                status.DatePosted, //preserve original date
                status.Comments != null
                    ? status.Comments.Select(c => new CommentDto(
                        c.Id,
                        c.TheComment,
                        c.Likes,
                        c.PostId
                    )).ToList()
                    : new List<CommentDto>(), //ensure it's never null
                status.Likes,
                status.UserId,
                status.User != null ? status.User.FullName : "Unknown User" //handles null user
            );

            return Results.Ok(updatedStatusDto);
        });

        // DELETE /statuses/{id}
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var status = await dbContext.Statuses.FindAsync(id);
            if (status == null)
            {
                return Results.NotFound($"Status with ID {id} not found.");
            }
            dbContext.Statuses.Remove(status);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}