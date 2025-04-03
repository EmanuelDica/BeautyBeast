using System.Net.Http.Headers;
using System.Net.Http.Json;
using BeautyBeast.Frontend.Dtos;

namespace BeautyBeast.Frontend.Services;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponseDto?> LoginAsync(string email, string password)
    {
        Console.WriteLine("[LOGIN] LoginAsync method was called");

        try
        {
            var requestDto = new LoginRequest { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("auth/login", requestDto);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                Console.WriteLine($"[LOGIN RESPONSE] Token: {loginResponse?.Token}, Role: {loginResponse?.Role}, UserId: {loginResponse?.UserId}");

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", loginResponse.Token);

                    return loginResponse;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LOGIN ERROR] {ex.Message}");
        }

        return null;
    }

    public void Logout()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}