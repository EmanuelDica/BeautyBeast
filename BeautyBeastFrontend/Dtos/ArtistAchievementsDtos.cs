using System.ComponentModel.DataAnnotations;

namespace BeautyBeastFrontend.Dtos;

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