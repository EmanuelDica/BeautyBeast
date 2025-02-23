using Blazored.LocalStorage;
using BeautyBeast.Frontend.Dtos;
using System.Net.Http.Json;

namespace BeautyBeast.Frontend.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<UserDto>>("users");
            return result ?? new List<UserDto>();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _httpClient.GetFromJsonAsync<UserDto>($"users/{id}");
            return user;
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            var authToken = await _localStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(authToken))
            {
                return null;
            }

            var userIdString = await _localStorage.GetItemAsync<string>("userId");
            if (!int.TryParse(userIdString, out var userId))
            {
                return null;
            }

            return await GetUserByIdAsync(userId);
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
}