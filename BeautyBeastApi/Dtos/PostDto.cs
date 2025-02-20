using System.ComponentModel.DataAnnotations;
using BeautyBeastApi.Dtos;
namespace BeautyBeastApi.Dtos;

public record class PostDto
(
    int Id,
    string Description,
    List<string> MediaUrls,
    DateTime DatePosted,
    List<CommentDto> Comments,
    int Likes,
    int ArtistId,
    string ArtistName
);

public record class CreatePostDto
(
    [Required][StringLength(300)] string Description,
    List<string>? MediaUrls,
    [Required]int ArtistId
);

public class EditPostDto
{
    [StringLength(300)]
    public string? Description { get; set; }

    public List<string>? MediaUrls { get; set; } = new List<string>();

    public EditPostDto() { }

    public EditPostDto(string? description, List<string>? mediaUrls = null)
    {
        Description = description;
        MediaUrls = mediaUrls ?? new List<string>();
    }
}