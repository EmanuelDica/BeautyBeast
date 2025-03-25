using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be a future date.";
    }
}

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
    [Required, FutureDate]DateTime BookingDateAndTime
);

public class EditBookingDto
{
    public int? ClientId { get; set; }
    public int? TreatmentId { get; set; }
    public DateTime? BookingDateAndTime { get; set; }
    public string? BookingStatus { get; set; }

    public EditBookingDto() { }

    public EditBookingDto(int? clientId, int? treatmentId, DateTime? bookingDateAndTime, string? bookingStatus)
    {
        ClientId = clientId;
        TreatmentId = treatmentId;
        BookingDateAndTime = bookingDateAndTime;
        BookingStatus = bookingStatus;
    }
}