namespace BeautyBeast.Frontend.Services;
using BeautyBeast.Frontend.Dtos;
using System.Net.Http.Json;

public class ArtistAchievementService
{
    private readonly HttpClient _httpClient;

    public ArtistAchievementService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ArtistAchievementDto>> GetAchievementsByArtistIdAsync(int artistId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<ArtistAchievementDto>>($"artist-achievements/{artistId}");
        return result ?? new List<ArtistAchievementDto>();
    }

    public async Task<bool> AddAchievementAsync(int artistId, CreateArtistAchievementDto newAchievement)
    {
        var response = await _httpClient.PostAsJsonAsync($"artist-achievements/{artistId}", newAchievement);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAchievementsAsync(int artistId, EditUserAchievementsDto editAchievements)
    {
        var response = await _httpClient.PutAsJsonAsync($"artist-achievements/{artistId}", editAchievements);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAchievementAsync(int achievementId)
    {
        var response = await _httpClient.DeleteAsync($"artist-achievements/{achievementId}");
        return response.IsSuccessStatusCode;
    }
}