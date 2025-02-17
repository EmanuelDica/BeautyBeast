namespace BeautyBeastServer.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BeautyBeastServer.Dtos;
using Blazored.LocalStorage;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {    
            var requestDto = new LoginRequestDto(email, password);
            var response = await _httpClient.PostAsJsonAsync("auth/login", requestDto);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                if (loginResponse?.Token != null)
                {
                    await _localStorage.SetItemAsync("authToken", loginResponse.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
                    return true;
                }
            }
        }    
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
        }
            return false;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}