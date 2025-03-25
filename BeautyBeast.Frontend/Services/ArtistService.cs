namespace BeautyBeast.Frontend.Services;
using BeautyBeast.Frontend.Dtos;
using System.Net.Http.Json;

public class ArtistService
{
    private readonly HttpClient _httpClient;

    public ArtistService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ArtistDto>> GetArtistsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ArtistDto>>("artists");
        return result ?? new List<ArtistDto>();
    }

    public async Task<ArtistDto?> GetArtistByIdAsync(int id)
    {
        var artist = await _httpClient.GetFromJsonAsync<ArtistDto>($"artists/{id}");
        return artist ?? null;
    }

    public async Task<ArtistDto?> CreateArtistAsync(CreateArtistDto newArtist)    
    {
        var response = await _httpClient.PostAsJsonAsync("artists", newArtist);
        if (response.IsSuccessStatusCode)
        {
            var thisArtist = await response.Content.ReadFromJsonAsync<ArtistDto>();
            return thisArtist;
        }
        return null;
    }

    public async Task<bool> UpdateArtistAsync(int id, EditArtistDto updatedArtist)
    {
        var response = await _httpClient.PutAsJsonAsync($"artists/{id}", updatedArtist);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteArtistAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"artists/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<TreatmentDto>> GetArtistTreatmentsAsync(int artistId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<TreatmentDto>>($"artists/{artistId}/treatments");
        return result ?? new List<TreatmentDto>();
    }

    public async Task<List<PostDto>> GetArtistPostsAsync(int artistId)
    {
        var result = await _httpClient.GetFromJsonAsync<List<PostDto>>($"artists/{artistId}/posts");
        return result ?? new List<PostDto>();
    }

    public async Task<bool> IsArtistAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"artists/{userId}/achievements");
        return response.IsSuccessStatusCode && (await response.Content.ReadFromJsonAsync<List<ArtistAchievementDto>>())?.Any() == true;
    }

    public async Task<List<ArtistAchievementDto>> GetAchievementsByArtistIdAsync(int artistId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<ArtistAchievementDto>>($"artists/{artistId}/achievements");
        return response ?? new List<ArtistAchievementDto>();
    }
}