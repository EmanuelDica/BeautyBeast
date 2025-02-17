namespace BeautyBeastApi.Dtos.TreatmentsDtos;

public record class TreatmentDto
(
    int Id,
    string Name,
    string Description,
    string PreCareInstructions,
    string AfterCareInstructions,
    string ConsentFormUrl,
    DateOnly BookingDate,
    TimeOnly StartTime,
    TimeSpan Duration,
    int ArtistId,
    string ArtistName
);