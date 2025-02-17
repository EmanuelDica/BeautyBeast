using System.ComponentModel.DataAnnotations;
namespace BeautyBeastServer.Dtos;

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

public record class EditPostDto
(
    [StringLength(300)] string? Description,
    List<string>? MediaUrls
);