using System.ComponentModel.DataAnnotations;

namespace BeautyBeast.Frontend.Dtos;

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

public record class EditTreatmentDto
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