namespace BeautyBeastApi.Entities;

public class Booking
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
    public int TreatmentId { get; set; }
    public Treatment? Treatment { get; set; }
    public DateTime BookingDateAndTime { get; set; }
    public string BookingStatus { get; set; } = "Pending";
}