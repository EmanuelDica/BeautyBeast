namespace BeautyBeastFrontend.Models;

public class ArtistAchievement
{
    public int Id { get; set; }
    public int ArtistId { get; set; }
    public required string Achievement { get; set; }
    public Artist? Artist { get; set; }
}