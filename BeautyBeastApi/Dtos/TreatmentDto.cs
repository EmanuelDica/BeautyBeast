using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;

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

public record class CreateTreatmentDto
(
    [Required]string Name,
    [Required]string Description,
    [Required]string PreCareInstructions,
    [Required]string AfterCareInstructions,
    [Required]string ConsentFormUrl,
    [Required] DateOnly BookingDate,
    [Required] TimeOnly StartTime,
    [Required] TimeSpan Duration,
    [Required]int ArtistId
);

public class EditTreatmentDto
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    [Required] public string PreCareInstructions { get; set; } = string.Empty;
    [Required] public string AfterCareInstructions { get; set; } = string.Empty;
    [Required] public string ConsentFormUrl { get; set; } = string.Empty;
    [Required] public DateOnly BookingDate { get; set; }
    [Required] public TimeOnly StartTime { get; set; }
    [Required] public TimeSpan Duration { get; set; }
    [Required] public int ArtistId { get; set; }

    public EditTreatmentDto() { }

    public EditTreatmentDto(string name, string description, string preCareInstructions, string afterCareInstructions, 
                            string consentFormUrl, DateOnly bookingDate, TimeOnly startTime, TimeSpan duration, int artistId)
    {
        Name = name;
        Description = description;
        PreCareInstructions = preCareInstructions;
        AfterCareInstructions = afterCareInstructions;
        ConsentFormUrl = consentFormUrl;
        BookingDate = bookingDate;
        StartTime = startTime;
        Duration = duration;
        ArtistId = artistId;
    }
}