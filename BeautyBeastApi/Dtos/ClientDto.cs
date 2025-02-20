using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;
public record class ClientDto(
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined,
    List<BookingDto> Bookings,
    List<StatusDto> Statuses
) : UserDto(Id, FullName, Email, ProfilePictureUrl, DateJoined);

public record class CreateClientDto(
    [Required][StringLength(50)] string FullName,
    [Required][StringLength(30)] string Email,
    string ProfilePictureUrl
) : CreateUserDto(FullName, Email, ProfilePictureUrl);

public class EditClientDto
{
    [Required]
    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
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