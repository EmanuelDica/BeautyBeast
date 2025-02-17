using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos.BookingDtos;
using BeautyBeastApi.Dtos.CommentDtos;
using BeautyBeastApi.Dtos.StatusDtos;
using BeautyBeastApi.Dtos.UserDtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;
namespace BeautyBeastApi.Endpoints;
public static class ClientsEndpoints
{
    const string getClientEndpointName = "GetClient";

    public static RouteGroupBuilder MapClientsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("clients").WithParameterValidation();

        // GET /clients
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var clients = await dbContext.Clients
                .Include(c => c.Bookings)
                .Include(c => c.Statuses)
                .ThenInclude(s => s.Comments)
                .ToListAsync(); // Load everything into memory first

            var clientDtos = clients.Select(c => new ClientDto(
                c.Id,
                c.FullName,
                c.Email,
                c.ProfilePictureUrl,
                c.DateJoined,
                c.Bookings.Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    c.FullName,
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.Status
                )).ToList(),
                c.Statuses.Select(s => new StatusDto(
                    s.Id,
                    s.Text,
                    s.DatePosted,
                    s.Comments.Select(comm => new CommentDto(
                        comm.Id,
                        comm.TheComment,
                        comm.Likes,
                        comm.PostId
                    )).ToList(),
                    s.Likes,
                    s.UserId,
                    s.User != null ? s.User.FullName : "Unknown User"
                )).ToList()
            )).ToList();

            return Results.Ok(clientDtos);
        });

        // GET /clients/{id}
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var client = await dbContext.Clients
                .Include(c => c.Bookings)
                .Include(c => c.Statuses)
                .ThenInclude(s => s.Comments)
                .FirstOrDefaultAsync(c => c.Id == id); // Load into memory first

            if (client == null)
            {
                return Results.NotFound($"Client with ID {id} not found.");
            }

            var clientDto = new ClientDto(
                client.Id,
                client.FullName,
                client.Email,
                client.ProfilePictureUrl,
                client.DateJoined,
                client.Bookings.Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    client.FullName,
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.Status
                )).ToList(),
                client.Statuses.Select(s => new StatusDto(
                    s.Id,
                    s.Text,
                    s.DatePosted,
                    s.Comments.Select(comm => new CommentDto(
                        comm.Id,
                        comm.TheComment,
                        comm.Likes,
                        comm.PostId
                    )).ToList(),
                    s.Likes,
                    client.Id,
                    client.FullName
                )).ToList()
            );

            return Results.Ok(clientDto);
        })
        .WithName(getClientEndpointName);

        // POST /clients
        group.MapPost("/", async (CreateClientDto newClient, BeautyBeastContext dbContext) =>
        {
            DateTime dateJoined = DateTime.UtcNow;

            Client client = new()
            {
                FullName = newClient.FullName,
                Email = newClient.Email,
                ProfilePictureUrl = newClient.ProfilePictureUrl,
                DateJoined = dateJoined
            };

            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            var clientDto = new ClientDto(
                client.Id,
                client.FullName,
                client.Email,
                client.ProfilePictureUrl,
                client.DateJoined,
                new List<BookingDto>(), //empty initially
                new List<StatusDto>() // empty initially
            );

            return Results.CreatedAtRoute(getClientEndpointName, new { id = client.Id }, clientDto);
        });

        // PUT /clients/{id}
        group.MapPut("/{id}", async (int id, EditClientDto editClient, BeautyBeastContext dbContext) =>
        {
            var client = await dbContext.Clients
                .Include(c => c.Bookings)
                .Include(c => c.Statuses)
                .ThenInclude(s => s.Comments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return Results.NotFound($"Client with ID {id} not found.");
            }

            // Preserve old values if new ones are not provided
            client.FullName = editClient.FullName ?? client.FullName;
            client.Email = editClient.Email ?? client.Email;
            client.ProfilePictureUrl = editClient.ProfilePictureUrl ?? client.ProfilePictureUrl;

            await dbContext.SaveChangesAsync();

            // Map to ClientDto for response
            var updatedClientDto = new ClientDto(
                client.Id,
                client.FullName,
                client.Email,
                client.ProfilePictureUrl,
                client.DateJoined,
                client.Bookings.Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    client.FullName,
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.Status
                )).ToList(),
                client.Statuses.Select(s => new StatusDto(
                    s.Id,
                    s.Text,
                    s.DatePosted,
                    s.Comments.Select(c => new CommentDto(
                        c.Id,
                        c.TheComment,
                        c.Likes,
                        c.PostId
                    )).ToList(),
                    s.Likes,
                    client.Id,
                    client.FullName
                )).ToList()
            );

            return Results.Ok(updatedClientDto);
        });

        // DELETE /clients/{id}
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var client = await dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                return Results.NotFound($"Client with ID {id} not found.");
            }

            dbContext.Clients.Remove(client);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}