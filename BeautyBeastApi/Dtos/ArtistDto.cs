using System.ComponentModel.DataAnnotations;
namespace BeautyBeastApi.Dtos;

public record class ArtistDto(
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined,
    string Role,
    string Bio,
    List<ArtistAchievementDto> Achievements,
    List<PostDto> Posts,
    List<TreatmentDto> Treatments
) : UserDto(Id, FullName, Email, ProfilePictureUrl, DateJoined);

public record class CreateArtistDto(
    [Required]
    [StringLength(50, ErrorMessage = "FullName cannot exceed 50 characters.")] string FullName,
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(30, ErrorMessage = "Email cannot exceed 30 characters.")] string Email,
    [Required] string Password,
    [Required] string Role = "Artist",
    string? ProfilePictureUrl = null,
    string? Bio = null,
    List<ArtistAchievementDto>? Achievements = null
) : CreateUserDto(FullName, Email, ProfilePictureUrl ?? string.Empty, Password, Role);

public class EditArtistDto
{
    [Required]
    [StringLength(50, ErrorMessage = "FullName cannot exceed 50 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(30, ErrorMessage = "Email cannot exceed 30 characters.")]
    public string Email { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }
    
    public string? Bio { get; set; }

    public string? Role { get; set; }
    public string? Password { get; set; }

    public List<ArtistAchievementDto>? Achievements { get; set; } = new();

    public EditArtistDto() {}

    public EditArtistDto(string fullName, string email, string? profilePictureUrl, string? bio, string? role = null, string? password = null, List<ArtistAchievementDto>? achievements = null)
    {
        FullName = fullName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
        Bio = bio ?? string.Empty;
        Role = role;
        Password = password;
        Achievements = achievements ?? new List<ArtistAchievementDto>();
    }
}