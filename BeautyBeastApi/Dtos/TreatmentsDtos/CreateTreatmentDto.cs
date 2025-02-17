using System.ComponentModel.DataAnnotations;
namespace BeautyBeastApi.Dtos.TreatmentsDtos;

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