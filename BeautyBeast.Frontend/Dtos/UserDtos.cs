using System.ComponentModel.DataAnnotations;

namespace BeautyBeast.Frontend.Dtos;

public record class UserDto( 
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined
);

public record class EditUsersDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string ProfilePictureUrl
);

public record class CreateUserDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string? ProfilePictureUrl
);