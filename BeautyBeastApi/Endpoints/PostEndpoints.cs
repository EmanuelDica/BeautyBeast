using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos.PostDtos;
using BeautyBeastApi.Dtos.CommentDtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeautyBeastApi.Endpoints;
public static class PostsEndpoints
{
    const string getPostEndpointName = "GetPost";

    public static RouteGroupBuilder MapPostsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("posts").WithParameterValidation();

        // GET /posts
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var posts = await dbContext.Posts
                .Include(p => p.Artist)
                .Include(p => p.Comments)
                .Select(p => new PostDto(
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
                    p.Artist != null ? p.Artist.FullName : "Unknown Artist"
                ))
                .ToListAsync();

            return Results.Ok(posts);
        });

        //GET /post/id
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var post = await dbContext.Posts
                .Where(p => p.Id == id)
                .Include(p => p.Artist)
                .Include(p => p.Comments)
                .Select(p => new PostDto(
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
                    p.Artist != null ? p.Artist.FullName : "Unknown Artist"
                ))
                .FirstOrDefaultAsync();

            return post is null ? Results.NotFound($"Post with ID {id} not found.") : Results.Ok(post);
        })
        .WithName(getPostEndpointName);

        // POST /posts
        group.MapPost("/", async (CreatePostDto newPost, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists.FindAsync(newPost.ArtistId);
            if (artist == null)
            {
                return Results.BadRequest($"Artist with ID {newPost.ArtistId} not found.");
            }

            Post post = new()
            {
                Description = newPost.Description,
                MediaUrls = newPost.MediaUrls ?? new List<string>(),
                DatePosted = DateTime.UtcNow,
                Likes = 0, //start with zero likes
                ArtistId = newPost.ArtistId,
                Artist = artist
            };

            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();

            var postDto = new PostDto(
                post.Id,
                post.Description,
                post.MediaUrls,
                post.DatePosted,
                new List<CommentDto>(), //empty initially
                post.Likes,
                post.ArtistId,
                artist.FullName
            );

            return Results.CreatedAtRoute(getPostEndpointName, new { id = post.Id }, postDto);
        });

        // PUT /posts/{id}
        group.MapPut("/{id}", async (int id, EditPostDto editPost, BeautyBeastContext dbContext) =>
        {
            var post = await dbContext.Posts
                .Include(p => p.Artist)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return Results.NotFound($"Post with ID {id} not found.");
            }

            //again, preserve old values if new ones are not provided
            post.Description = editPost.Description ?? post.Description;
            post.MediaUrls = editPost.MediaUrls ?? post.MediaUrls;

            await dbContext.SaveChangesAsync();

            var updatedPostDto = new PostDto(
                post.Id,
                post.Description,
                post.MediaUrls,
                post.DatePosted,
                post.Comments.Select(c => new CommentDto(
                    c.Id,
                    c.TheComment,
                    c.Likes,
                    c.PostId
                )).ToList(),
                post.Likes,
                post.ArtistId,
                post.Artist?.FullName ?? "Unknown Artist"
            );

            return Results.Ok(updatedPostDto);
        });

        // DELETE /posts/{id}
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var post = await dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return Results.NotFound($"Post with ID {id} not found.");
            }

            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}