namespace BeautyBeastApi.Dtos.BookingDtos;

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