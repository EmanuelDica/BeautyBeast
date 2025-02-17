using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos.ArtistAchievementsDtos;

public record class EditUserAchievementsDto(
    [Required] List<string> Achievements
);