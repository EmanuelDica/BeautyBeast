using BeautyBeastApi.Dtos.ArtistAchievementsDtos;
using BeautyBeastApi.Dtos.PostDtos;
using BeautyBeastApi.Dtos.TreatmentsDtos;

namespace BeautyBeastApi.Dtos.UserDtos;

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