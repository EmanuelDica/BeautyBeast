namespace BeautyBeastApi.Dtos.BookingDtos;
using System.ComponentModel.DataAnnotations;

public record class CreateBookingDto
(
    [Required]int ClientId,
    [Required]int TreatmentId,
    [Required]DateTime BookingDateAndTime
);