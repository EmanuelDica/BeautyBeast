using System.ComponentModel.DataAnnotations;
namespace BeautyBeastServer.Dtos;

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

public record class EditArtistDto(
    [Required][StringLength(50)]string FullName,
    [Required][StringLength(30)]string Email,
    string? ProfilePictureUrl,
    string? Bio,
    List<ArtistAchievementDto>? Achievements
);