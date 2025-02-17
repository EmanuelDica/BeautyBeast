using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos.UserDtos;

public record class EditUsersDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string ProfilePictureUrl
);