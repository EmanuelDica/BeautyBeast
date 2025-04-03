namespace BeautyBeastApi.Endpoints;

using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;

public static class UsersEndpoints
{
    const string getUserEndpointName = "GetUser";

    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users").WithParameterValidation();
        // GET /users
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var users = await dbContext.Users
                .Select(u => new UserDto(
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.ProfilePictureUrl,
                    u.DateJoined,
                    u.Role
                ))
                .ToListAsync();

            return Results.Ok(users);
        });

        //GET /users/id
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var user = await dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto(
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.ProfilePictureUrl,
                    u.DateJoined,
                    u.Role
                ))
                .FirstOrDefaultAsync();

            return user is null ? Results.NotFound($"User with ID {id} not found.") : Results.Ok(user);
        })
        .WithName(getUserEndpointName);

        // POST /new user
        group.MapPost("/", async (CreateUserDto newUser, BeautyBeastContext dbContext) =>
        {
            DateTime dateJoined = DateTime.UtcNow;

            User user = new()
            {
                FullName = newUser.FullName,
                Email = newUser.Email,
                ProfilePictureUrl = newUser.ProfilePictureUrl,
                DateJoined = dateJoined,
                HashedPassword = PasswordHelper.HashPassword(newUser.HashedPassword),
                Role = newUser.Role
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var userDto = new UserDto(user.Id, user.FullName, user.Email, user.ProfilePictureUrl, user.DateJoined, user.Role);

            return Results.CreatedAtRoute(getUserEndpointName, new { id = user.Id }, userDto);
        });

        // PUT /users
        group.MapPut("/{id}", async (int id, EditUsersDto editUser, BeautyBeastContext dbContext) =>
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            // Preserve old values if new ones are not provided
            user.FullName = editUser.FullName ?? user.FullName;
            user.Email = editUser.Email ?? user.Email;
            user.ProfilePictureUrl = editUser.ProfilePictureUrl ?? user.ProfilePictureUrl;

            await dbContext.SaveChangesAsync();

            var updatedUserDto = new UserDto(user.Id, user.FullName, user.Email, user.ProfilePictureUrl, user.DateJoined, user.Role);

            return Results.Ok(updatedUserDto);
        });

        //DELETE / users

        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}