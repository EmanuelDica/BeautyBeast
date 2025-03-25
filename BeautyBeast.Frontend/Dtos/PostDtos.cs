using System.ComponentModel.DataAnnotations;

namespace BeautyBeast.Frontend.Dtos;

public record class PostDto
(
    int Id,
    string Description,
    string MediaUrl,
    DateTime DatePosted,
    List<CommentDto> Comments,
    int Likes,
    int ArtistId,
    string ArtistName
);

public record class CreatePostDto
(
    [Required][StringLength(300)] string Description,
    [Required] string MediaUrl,
    [Required] int ArtistId
);


public class EditPostDto
{
    [StringLength(300)]
    public string? Description { get; set; } = string.Empty;

    [Required]
    public string MediaUrl { get; set; }

    public EditPostDto() 
    { 
        MediaUrl = string.Empty;
    }

    public EditPostDto(string? description, string mediaUrl)
    {
        Description = description;
        MediaUrl = mediaUrl;
    }
}