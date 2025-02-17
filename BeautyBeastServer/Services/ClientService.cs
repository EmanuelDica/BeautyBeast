namespace BeautyBeastServer.Services;
using System.Net.Http.Json;
using BeautyBeastServer.Dtos;

public class ClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Fetch all clients
    public async Task<List<ClientDto>> GetClientsAsync()
    {
    var result = await _httpClient.GetFromJsonAsync<List<ClientDto>>("clients");
    return result ?? new List<ClientDto>();
    }

    // Fetch a single client by ID
    public async Task<ClientDto> GetClientByIdAsync(int id)
    {
        var client = await _httpClient.GetFromJsonAsync<ClientDto>($"clients/{id}");
        if (client == null)
        {
            throw new Exception($"Client with ID {id} not found.");
        }
        return client;
    }

    // Create a new client
    public async Task<bool> CreateClientAsync(CreateClientDto newClient)
    {
        var response = await _httpClient.PostAsJsonAsync("clients", newClient);
        return response.IsSuccessStatusCode;
    }

    // Update an existing client
    public async Task<bool> UpdateClientAsync(int id, EditClientDto editClient)
    {
        var response = await _httpClient.PutAsJsonAsync($"clients/{id}", editClient);
        return response.IsSuccessStatusCode;
    }

    // Delete a client
    public async Task<bool> DeleteClientAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"clients/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> IsClientAsync(int userId)
{
    var response = await _httpClient.GetAsync($"clients/{userId}/bookings");
    return response.IsSuccessStatusCode && (await response.Content.ReadFromJsonAsync<List<BookingDto>>())?.Any() == true;
}
}