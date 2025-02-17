namespace BeautyBeast.Frontend.Services;
using BeautyBeast.Frontend.Dtos;
using System.Net.Http.Json;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<UserDto>>("users");
        return result ?? new List<UserDto>();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _httpClient.GetFromJsonAsync<UserDto>($"users/{id}");
        return user ?? null;
    }

    public async Task<bool> CreateUserAsync(CreateUserDto newUser)
    {
        var response = await _httpClient.PostAsJsonAsync("users", newUser);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(int id, EditUsersDto updatedUser)
    {
        var response = await _httpClient.PutAsJsonAsync($"users/{id}", updatedUser);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"users/{id}");
        return response.IsSuccessStatusCode;
    }
}