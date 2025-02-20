using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;

public record class ArtistAchievementDto(
    int Id,
    string Achievement
);

public record class CreateArtistAchievementDto(
    [Required] string Achievement
);

public class EditUserAchievementsDto
{
    [Required]
    public List<string> Achievements { get; set; }

    public EditUserAchievementsDto()
    {
        Achievements = new List<string>(); 
    }

    public EditUserAchievementsDto(List<string> achievements)
    {
        Achievements = achievements ?? new List<string>(); 
    }
}