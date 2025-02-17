using System.ComponentModel.DataAnnotations;
namespace BeautyBeastApi.Dtos.CommentDtos;

public record class EditCommentDto(
    [Required] string TheComment
);