namespace BeautyBeastApi.Dtos.PostDtos;
using System.ComponentModel.DataAnnotations;

public record class EditPostDto
(
    [StringLength(300)] string? Description,
    List<string>? MediaUrls
);