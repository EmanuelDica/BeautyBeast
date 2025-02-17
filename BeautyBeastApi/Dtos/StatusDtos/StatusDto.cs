namespace BeautyBeastApi.Dtos.StatusDtos;

using BeautyBeastApi.Dtos.CommentDtos;
using BeautyBeastApi.Entities;

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