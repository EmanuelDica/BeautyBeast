namespace BeautyBeastApi.Dtos;

using System.ComponentModel.DataAnnotations;

public record class StatusDto
(
    int Id,
    string Text,
    DateTime DatePosted,
    List<CommentDto> Comments,
    int Likes,
    int UserId,
    string UserName
);

public record class CreateStatusDto
(
    [Required][StringLength(500)] string Text,
    int UserId
);

public class EditStatusDto
{
    [Required]
    [StringLength(500)]
    public required string Text { get; set; }

    public int UserId { get; set; }

    public EditStatusDto() { }

    public EditStatusDto(string text, int userId)
    {
        Text = text;
        UserId = userId;
    }
}