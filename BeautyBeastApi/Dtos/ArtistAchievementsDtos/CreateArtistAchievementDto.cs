using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos.ArtistAchievementsDtos;

public record class CreateArtistAchievementDto(
    [Required] string Achievement
);