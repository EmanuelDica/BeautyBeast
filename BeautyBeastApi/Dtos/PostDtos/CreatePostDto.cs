namespace BeautyBeastApi.Dtos.PostDtos;
using System.ComponentModel.DataAnnotations;

public record class CreatePostDto
(
    [Required][StringLength(300)] string Description,
    List<string>? MediaUrls,
    [Required]int ArtistId
);