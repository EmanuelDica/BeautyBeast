namespace BeautyBeastServer.Models;

public class Status
{
    public int Id { get; set; }
    public required string Text { get; set; } = string.Empty;
    public DateTime DatePosted { get; set; } = DateTime.UtcNow;
    public List<Comment> Comments { get; set; } = new();
    public int Likes { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}