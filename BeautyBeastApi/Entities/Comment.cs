namespace BeautyBeastApi.Entities;

public class Comment
{
    public int Id { get; set;}
    public required string TheComment { get; set; }
    public int Likes { get; set; }
    public int PostId { get; set; }
    public Post? Post { get; set; }
}