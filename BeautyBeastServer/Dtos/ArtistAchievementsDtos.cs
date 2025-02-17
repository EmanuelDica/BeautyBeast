using System.ComponentModel.DataAnnotations;
namespace BeautyBeastServer.Dtos;

public record class ArtistAchievementDto(
    int Id,
    string Achievement
);

public record class CreateArtistAchievementDto(
    [Required] string Achievement
);

public record class EditUserAchievementsDto(
    [Required] List<string> Achievements
);