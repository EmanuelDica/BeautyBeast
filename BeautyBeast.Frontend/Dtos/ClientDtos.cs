using System.ComponentModel.DataAnnotations;
namespace BeautyBeast.Frontend.Dtos;

public record class ClientDto(
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined,
    string Role,
    List<BookingDto> Bookings,
    List<StatusDto> Statuses
) : UserDto(Id, FullName, Email, ProfilePictureUrl, DateJoined);

public record class CreateClientDto(
    [Required]
    [StringLength(50, ErrorMessage = "FullName cannot exceed 50 characters.")] 
    string FullName,
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(30, ErrorMessage = "Email cannot exceed 30 characters.")] 
    string Email,
    string? ProfilePictureUrl,
    [Required] string Password,
    [Required] string Role
) : CreateUserDto(FullName, Email, ProfilePictureUrl ?? string.Empty, Password, Role);

public class EditClientDto
{
    [Required]
    [StringLength(50, ErrorMessage = "FullName cannot exceed 50 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(30, ErrorMessage = "Email cannot exceed 30 characters.")]
    public string Email { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }

    public EditClientDto() { }

    public EditClientDto(string fullName, string email, string? profilePictureUrl = null)
    {
        FullName = fullName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
    }
}