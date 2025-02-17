namespace BeautyBeastServer.Services;
using System.Net.Http.Json;
using BeautyBeastServer.Dtos;

public class TreatmentService
{
    private readonly HttpClient _httpClient;

    public TreatmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TreatmentDto>> GetAllTreatmentsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TreatmentDto>>("treatments");
        return result ?? new List<TreatmentDto>();
    }

    public async Task<TreatmentDto?> GetTreatmentByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TreatmentDto?>($"treatments/{id}");
    }

    public async Task<bool> CreateTreatmentAsync(CreateTreatmentDto newTreatment)
    {
        var response = await _httpClient.PostAsJsonAsync("treatments", newTreatment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateTreatmentAsync(int id, EditTreatmentDto editTreatment)
    {
        var response = await _httpClient.PutAsJsonAsync($"treatments/{id}", editTreatment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTreatmentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"treatments/{id}");
        return response.IsSuccessStatusCode;
    }
}