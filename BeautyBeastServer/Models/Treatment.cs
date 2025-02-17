namespace BeautyBeastServer.Models;

public class Treatment
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required string PreCareInstructions { get; set; } = string.Empty;
    public required string AfterCareInstructions { get; set; } = string.Empty;
    public required string ConsentFormUrl { get; set; } = string.Empty;
    public required DateOnly BookingDate;
    public required TimeOnly StartTime;
    public required TimeSpan Duration;
    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }
}