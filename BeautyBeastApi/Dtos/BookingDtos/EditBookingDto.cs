namespace BeautyBeastApi.Dtos.BookingDtos;

public record class EditBookingDto
(
    int? ClientId,
    int? TreatmentId,
    DateTime? BookingDateAndTime,
    string? BookingStatus
);