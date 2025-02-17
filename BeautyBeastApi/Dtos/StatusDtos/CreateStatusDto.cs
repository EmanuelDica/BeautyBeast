using System.ComponentModel.DataAnnotations;
namespace BeautyBeastApi.Dtos.StatusDtos;

public record class CreateStatusDto
(
    [Required][StringLength(500)] string Text,
    int UserId
);