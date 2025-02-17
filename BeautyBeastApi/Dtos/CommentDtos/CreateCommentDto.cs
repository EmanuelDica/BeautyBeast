using System.ComponentModel.DataAnnotations;
namespace BeautyBeastApi.Dtos.CommentDtos;

public record class CreateCommentDto(
    [Required] string TheComment,
    [Required] int PostId
);