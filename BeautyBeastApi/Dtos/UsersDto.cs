using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;

public record class UserDto( 
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined
);

public class EditUsersDto
{
    [Required][StringLength(50)] public string FullName { get; set; } = string.Empty;
    [Required][StringLength(30)] public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }

    public EditUsersDto() { }

    public EditUsersDto(string fullName, string email, string? profilePictureUrl)
    {
        FullName = fullName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
    }
}

public record class CreateUserDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string? ProfilePictureUrl
);