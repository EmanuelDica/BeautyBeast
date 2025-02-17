using System.ComponentModel.DataAnnotations;

namespace BeautyBeastFrontend.Dtos;

public record class BookingDto
(
    int Id,
    int ClientId,
    string ClientName,
    int TreatmentId,
    string TreatmentName,
    DateTime BookingDateAndTime,
    string BookingStatus
);

public record class CreateBookingDto
(
    [Required]int ClientId,
    [Required]int TreatmentId,
    [Required]DateTime BookingDateAndTime
);

public record class EditBookingDto
(
    int? ClientId,
    int? TreatmentId,
    DateTime? BookingDateAndTime,
    string? BookingStatus
);