namespace BeautyBeastServer.Models;

public class Post
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public required List<string> MediaUrls { get; set; } = [];
    public DateTime DatePosted { get; set; } = DateTime.UtcNow;
    public List<Comment> Comments { get; set; } = [];
    public int Likes { get; set; }
    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }
}