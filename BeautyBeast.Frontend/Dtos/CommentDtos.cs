using System.ComponentModel.DataAnnotations;

namespace BeautyBeast.Frontend.Dtos;

public record class CommentDto(
    int Id,
    string TheComment,
    int Likes,
    int PostId 
);

public record class CreateCommentDto(
    [Required] string TheComment,
    [Required] int PostId
);

public record class EditCommentDto(
    [Required] string TheComment
);