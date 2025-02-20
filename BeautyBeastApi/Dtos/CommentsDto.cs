using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;

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


public class EditCommentDto
{
    [Required]
    public string TheComment { get; set; } = string.Empty;

    public EditCommentDto() { }

    public EditCommentDto(string theComment)
    {
        TheComment = theComment;
    }
}