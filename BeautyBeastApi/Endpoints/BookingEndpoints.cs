using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos.BookingDtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;
namespace BeautyBeastApi.Endpoints;

public static class BookingsEndpoints
{
    const string getBookingEndpointName = "GetBooking";

    public static RouteGroupBuilder MapBookingsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("bookings").WithParameterValidation();

        // GET /bookings
        group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var bookings = await dbContext.Bookings
                .Include(b => b.Client)
                .Include(b => b.Treatment)
                .Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    b.Client != null ? b.Client.FullName : "Unknown Client",
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.Status
                ))
                .ToListAsync();

            return Results.Ok(bookings);
        });
        
        // GET /bookings/{id}
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var booking = await dbContext.Bookings
                .Where(b => b.Id == id)
                .Include(b => b.Client)
                .Include(b => b.Treatment)
                .Select(b => new BookingDto(
                    b.Id,
                    b.ClientId,
                    b.Client != null ? b.Client.FullName : "Unknown Client",
                    b.TreatmentId,
                    b.Treatment != null ? b.Treatment.Name : "Unknown Treatment",
                    b.BookingDateAndTime,
                    b.Status
                ))
                .FirstOrDefaultAsync();

            return booking is null ? Results.NotFound($"Booking with ID {id} not found.") : Results.Ok(booking);
        })
        .WithName(getBookingEndpointName);

        // POST /bookings
        group.MapPost("/", async (CreateBookingDto newBooking, BeautyBeastContext dbContext) =>
        {
            var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == newBooking.ClientId);
            var treatment = await dbContext.Treatments.FirstOrDefaultAsync(t => t.Id == newBooking.TreatmentId);

            if (client == null)
            {
                return Results.BadRequest($"Client with ID {newBooking.ClientId} not found.");
            }
            if (treatment == null)
            {
                return Results.BadRequest($"Treatment with ID {newBooking.TreatmentId} not found.");
            }

            Booking booking = new()
            {
                ClientId = newBooking.ClientId,
                TreatmentId = newBooking.TreatmentId,
                BookingDateAndTime = newBooking.BookingDateAndTime
            };

            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();

            var bookingDto = new BookingDto(
                booking.Id,
                booking.ClientId,
                client.FullName ?? "Unknown Client",
                booking.TreatmentId,
                treatment.Name ?? "Unknown Treatment",
                booking.BookingDateAndTime,
                booking.Status
            );

            return Results.CreatedAtRoute(getBookingEndpointName, new { id = booking.Id }, bookingDto);
        });

        // PUT /bookings/{id}
        group.MapPut("/{id}", async (int id, EditBookingDto editBooking, BeautyBeastContext dbContext) =>
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return Results.NotFound($"Booking with ID {id} not found.");
            }

            booking.ClientId = editBooking.ClientId ?? booking.ClientId;
            booking.TreatmentId = editBooking.TreatmentId ?? booking.TreatmentId;
            booking.BookingDateAndTime = editBooking.BookingDateAndTime ?? booking.BookingDateAndTime;
            booking.Status = editBooking.BookingStatus ?? booking.Status;

            await dbContext.SaveChangesAsync();
            return Results.Ok(new BookingDto(
                booking.Id,
                booking.ClientId,
                booking.Client != null ? booking.Client.FullName : "Unknown Client",
                booking.TreatmentId,
                booking.Treatment != null ? booking.Treatment.Name : "Unknown Treatment",
                booking.BookingDateAndTime,
                booking.Status
            ));
        });

        // DELETE /bookings/{id}
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var booking = await dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return Results.NotFound($"Booking with ID {id} not found.");
            }

            dbContext.Bookings.Remove(booking);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        return group;
    }
}