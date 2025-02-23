using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;
namespace BeautyBeastApi.Endpoints;
public static class CommentsEndpoints
{
    const string getCommentEndpointName = "GetComment";

    public static RouteGroupBuilder MapCommentsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("comments").WithParameterValidation();

        // GET /comments
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var comments = await dbContext.Comments
                .Select(c => new CommentDto(
                    c.Id,
                    c.TheComment,
                    c.Likes,
                    c.PostId
                ))
                .ToListAsync();

            return Results.Ok(comments);
        });
        // GET /comments/{id}
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var comment = await dbContext.Comments
                .Where(c => c.Id == id)
                .Select(c => new CommentDto(
                    c.Id,
                    c.TheComment,
                    c.Likes,
                    c.PostId
                ))
                .FirstOrDefaultAsync();

            return comment is null ? Results.NotFound($"Comment with ID {id} not found.") : Results.Ok(comment);
        })
        .WithName(getCommentEndpointName);

        // POST /comments
        group.MapPost("/", async (CreateCommentDto newComment, BeautyBeastContext dbContext) =>
        {
            var post = await dbContext.Posts.FindAsync(newComment.PostId);
            if(post == null)
            {
                return Results.BadRequest($"Post with ID {newComment.PostId} not found.");
            }

            Comment comment = new()
            {
                TheComment = newComment.TheComment,
                Likes = 0, //start with zero likes
                PostId = newComment.PostId
            };

            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(getCommentEndpointName, new { id = comment.Id }, new CommentDto(
                comment.Id,
                comment.TheComment,
                comment.Likes,
                comment.PostId
            ));
        });

        // PUT /comments/{id}
        group.MapPut("/{id}", async (int id, EditCommentDto editComment, BeautyBeastContext dbContext) =>
        {
            var comment = await dbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return Results.NotFound($"Comment with ID {id} not found.");
            }

            //preserve old values if new ones are not provided
            comment.TheComment = editComment.TheComment ?? comment.TheComment;

            await dbContext.SaveChangesAsync();

            var updatedCommentDto = new CommentDto(comment.Id, comment.TheComment, comment.Likes, comment.PostId);

            return Results.Ok(updatedCommentDto);
        });

        // DELETE /comments/{id}
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var comment = await dbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return Results.NotFound($"Comment with ID {id} not found.");
            }

            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}