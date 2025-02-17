using System.ComponentModel.DataAnnotations;
using BeautyBeastApi.Dtos.ArtistAchievementsDtos;

namespace BeautyBeastApi.Dtos.UserDtos;

public record class CreateArtistDto(
    [Required][StringLength(50)] string FullName,
    [Required][StringLength(30)] string Email,
    string ProfilePictureUrl,
    string Bio,
    List<ArtistAchievementDto> Achievements
) : CreateUserDto(FullName, Email, ProfilePictureUrl);