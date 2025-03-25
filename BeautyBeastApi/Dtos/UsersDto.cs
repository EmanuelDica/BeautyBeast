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
    [Required]
    [StringLength(50, ErrorMessage = "FullName cannot exceed 50 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(30, ErrorMessage = "Email cannot exceed 30 characters.")]
    public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }

    public EditUsersDto() { }

    public EditUsersDto(string fullName, string email, string? profilePictureUrl = null)
    {
        FullName = fullName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
    }
}

public record class CreateUserDto(
    [Required]
    [StringLength(50, ErrorMessage = "FullName cannot exceed 50 characters.")] 
    string FullName,
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(30, ErrorMessage = "Email cannot exceed 30 characters.")] 
    string Email,
    [Required] string HashedPassword,
    [Required] string Role,
    string? ProfilePictureUrl = null
);