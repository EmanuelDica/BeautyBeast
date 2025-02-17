namespace BeautyBeastApi.Dtos.UserDtos;
using System.ComponentModel.DataAnnotations;

public record class EditClientDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string? ProfilePictureUrl
);