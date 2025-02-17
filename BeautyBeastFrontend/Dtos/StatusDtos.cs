using System.ComponentModel.DataAnnotations;

namespace BeautyBeastFrontend.Dtos;

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

public record class EditStatusDto
(
    [Required][StringLength(500)] string Text,
    int UserId
);