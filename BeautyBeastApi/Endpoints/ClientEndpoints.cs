using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos;
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
            // Load all clients with related Bookings, Treatments, Statuses, Comments, and Users
            var clients = await dbContext.Clients
                .Include(c => c.Bookings)
                    .ThenInclude(b => b.Treatment)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.Comments)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.User) 
                .ToListAsync();

            
            var clientDtos = clients.Select(c => new ClientDto(
                c.Id,
                c.FullName,
                c.Email,
                c.ProfilePictureUrl,
                c.DateJoined,
                c.Role,
                c.Bookings.Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    c.FullName,
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.BookingStatus
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
                    .ThenInclude(b => b.Treatment)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.Comments)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(c => c.Id == id);

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
                client.Role,
                client.Bookings.Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    client.FullName,
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.BookingStatus
                )).ToList(),
                client.Statuses != null
                    ? client.Statuses.Select(s => new StatusDto(
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
                    : new List<StatusDto>()
            );

            return Results.Ok(clientDto);
        }).WithName("GetClient");

        // POST /clients
        group.MapPost("/", async (CreateClientDto newClient, BeautyBeastContext dbContext) =>
        {
            string hashedPassword = PasswordHelper.HashPassword(newClient.Password);

            Client client = new()
            {
                FullName = newClient.FullName,
                Email = newClient.Email,
                ProfilePictureUrl = newClient.ProfilePictureUrl,
                DateJoined = DateTime.UtcNow,
                HashedPassword = hashedPassword,
                Role = newClient.Role ?? "Client",
                Bookings = new List<Booking>(),
                Statuses = new List<Status>()
            };

            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            var clientDto = new ClientDto(
                client.Id,
                client.FullName,
                client.Email,
                client.ProfilePictureUrl,
                client.DateJoined,
                client.Role,
                new List<BookingDto>(),
                new List<StatusDto>()
            );

            return Results.CreatedAtRoute("GetClient", new { id = client.Id }, clientDto);
        });

        // PUT /clients/{id}
        group.MapPut("/{id}", async (int id, EditClientDto editClient, BeautyBeastContext dbContext) =>
        {
            var client = await dbContext.Clients
                .Include(c => c.Bookings)
                    .ThenInclude(b => b.Treatment)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.Comments)
                .Include(c => c.Statuses)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return Results.NotFound($"Client with ID {id} not found.");
            }

            client.FullName = !string.IsNullOrEmpty(editClient.FullName) ? editClient.FullName : client.FullName;
            client.Email = !string.IsNullOrEmpty(editClient.Email) ? editClient.Email : client.Email;
            client.ProfilePictureUrl = editClient.ProfilePictureUrl ?? client.ProfilePictureUrl;

            await dbContext.SaveChangesAsync();

            var updatedClientDto = new ClientDto(
                client.Id,
                client.FullName,
                client.Email,
                client.ProfilePictureUrl,
                client.DateJoined,
                client.Role,
                client.Bookings.Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    client.FullName,
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.BookingStatus
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
                    s.UserId,
                    s.User != null ? s.User.FullName : "Unknown User"
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