using BeautyBeastApi.Data;
using BeautyBeastApi.Dtos.TreatmentsDtos;
using BeautyBeastApi.Entities;
using Microsoft.EntityFrameworkCore;
namespace BeautyBeastApi.Endpoints;
public static class TreatmentsEndpoints
{
    const string getTreatmentEndpointName = "GetTreatment";
    public static RouteGroupBuilder MapTreatmentsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("treatments").WithParameterValidation();

        // GET /treatments
       group.MapGet("/", async (BeautyBeastContext dbContext) =>
        {
            var treatments = await dbContext.Treatments
                .Include(t => t.Artist)
                .Select(t => new TreatmentDto(
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
                    t.Artist != null ? t.Artist.FullName : "Unknown Artist" //unlikely null artist val
                ))
                .ToListAsync();

            return Results.Ok(treatments);
        });

        // GET /treatments/{id}
        group.MapGet("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var treatment = await dbContext.Treatments
                .Where(t => t.Id == id)
                .Include(t => t.Artist)
                .Select(t => new TreatmentDto(
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
                    t.Artist != null ? t.Artist.FullName : "Unknown Artist" // same as above
                ))
                .FirstOrDefaultAsync();

            return treatment is null ? Results.NotFound($"Treatment with ID {id} not found.") : Results.Ok(treatment);
        })
        .WithName(getTreatmentEndpointName);

        // POST /treatments
        group.MapPost("/", async (CreateTreatmentDto newTreatment, BeautyBeastContext dbContext) =>
        {
            var artist = await dbContext.Artists.FindAsync(newTreatment.ArtistId);
            if (artist == null)
            {
                return Results.BadRequest($"Artist with ID {newTreatment.ArtistId} not found.");
            }

            Treatment treatment = new()
            {
                Name = newTreatment.Name,
                Description = newTreatment.Description,
                PreCareInstructions = newTreatment.PreCareInstructions,
                AfterCareInstructions = newTreatment.AfterCareInstructions,
                ConsentFormUrl = newTreatment.ConsentFormUrl,
                BookingDate = newTreatment.BookingDate,
                StartTime = newTreatment.StartTime,
                Duration = newTreatment.Duration,
                ArtistId = newTreatment.ArtistId,
                Artist = artist
            };

            dbContext.Treatments.Add(treatment);
            await dbContext.SaveChangesAsync();

            var treatmentDto = new TreatmentDto(
                treatment.Id,
                treatment.Name,
                treatment.Description,
                treatment.PreCareInstructions,
                treatment.AfterCareInstructions,
                treatment.ConsentFormUrl,
                treatment.BookingDate,
                treatment.StartTime,
                treatment.Duration,
                treatment.ArtistId,
                artist.FullName
            );

            return Results.CreatedAtRoute(getTreatmentEndpointName, new { id = treatment.Id }, treatmentDto);
        });

        // PUT /treatments/{id}
        group.MapPut("/{id}", async (int id, EditTreatmentDto editTreatment, BeautyBeastContext dbContext) =>
        {
            var treatment = await dbContext.Treatments
                .Include(t => t.Artist)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treatment == null)
            {
                return Results.NotFound($"Treatment with ID {id} not found.");
            }

            // Preserve old values if new ones are not provided
            treatment.Name = editTreatment.Name ?? treatment.Name;
            treatment.Description = editTreatment.Description ?? treatment.Description;
            treatment.PreCareInstructions = editTreatment.PreCareInstructions ?? treatment.PreCareInstructions;
            treatment.AfterCareInstructions = editTreatment.AfterCareInstructions ?? treatment.AfterCareInstructions;
            treatment.ConsentFormUrl = editTreatment.ConsentFormUrl ?? treatment.ConsentFormUrl;
            treatment.Duration = editTreatment.Duration;

            await dbContext.SaveChangesAsync();

            var updatedTreatmentDto = new TreatmentDto(
                treatment.Id,
                treatment.Name,
                treatment.Description,
                treatment.PreCareInstructions,
                treatment.AfterCareInstructions,
                treatment.ConsentFormUrl,
                treatment.BookingDate,
                treatment.StartTime,
                treatment.Duration,
                treatment.ArtistId,
                treatment.Artist != null ? treatment.Artist.FullName : "Unknown Artist" //same as above
            );

            return Results.Ok(updatedTreatmentDto);
        });

        // DELETE /treatments/{id}
        group.MapDelete("/{id}", async (int id, BeautyBeastContext dbContext) =>
        {
            var treatment = await dbContext.Treatments.FindAsync(id);
            if (treatment == null)
            {
                return Results.NotFound($"Treatment with ID {id} not found.");
            }

            dbContext.Treatments.Remove(treatment);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}