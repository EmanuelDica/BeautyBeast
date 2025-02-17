namespace BeautyBeastServer.Services;
using System.Net.Http.Json;
using BeautyBeastServer.Dtos;

public class StatusService
{
    private readonly HttpClient _httpClient;

    public StatusService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<StatusDto>> GetAllStatusesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<StatusDto>>("statuses");
        return result ?? new List<StatusDto>();
    }

    public async Task<StatusDto?> GetStatusByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<StatusDto?>($"statuses/{id}");
    }

    public async Task<bool> CreateStatusAsync(CreateStatusDto newStatus)
    {
        var response = await _httpClient.PostAsJsonAsync("statuses", newStatus);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateStatusAsync(int id, EditStatusDto editStatus)
    {
        var response = await _httpClient.PutAsJsonAsync($"statuses/{id}", editStatus);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteStatusAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"statuses/{id}");
        return response.IsSuccessStatusCode;
    }
}