using System.ComponentModel.DataAnnotations;
namespace BeautyBeastApi.Dtos;

public record class ArtistDto(
    int Id,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    DateTime DateJoined,
    string Bio,
    List<ArtistAchievementDto> Achievements,
    List<PostDto> Posts,
    List<TreatmentDto> Treatments
) : UserDto(Id, FullName, Email, ProfilePictureUrl, DateJoined);

public record class CreateArtistDto(
    [Required][StringLength(50)] string FullName,
    [Required][StringLength(30)] string Email,
    string ProfilePictureUrl,
    string Bio,
    List<ArtistAchievementDto> Achievements
) : CreateUserDto(FullName, Email, ProfilePictureUrl);

public class EditArtistDto
{
    [Required]
    [StringLength(50)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Email { get; set; } = string.Empty;

    public string? ProfilePictureUrl { get; set; }
    
    public string? Bio { get; set; }

    public List<ArtistAchievementDto>? Achievements { get; set; }

    public EditArtistDto()
    {
        Achievements = new List<ArtistAchievementDto>();
    }

    public EditArtistDto(string fullName, string email, string? profilePictureUrl, string? bio, List<ArtistAchievementDto>? achievements = null)
    {
        FullName = fullName;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
        Bio = bio;
        Achievements = achievements ?? new List<ArtistAchievementDto>();
    }
}