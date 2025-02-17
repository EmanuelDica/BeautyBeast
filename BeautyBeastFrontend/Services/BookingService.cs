namespace BeautyBeastFrontend.Services;
using BeautyBeastFrontend.Dtos;
using System.Net.Http.Json;

public class BookingService
{
    private readonly HttpClient _httpClient;

    public BookingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BookingDto>> GetAllBookingsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<BookingDto>>("bookings");
        return result ?? new List<BookingDto>();
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<BookingDto?>($"bookings/{id}");
    }

    public async Task<bool> CreateBookingAsync(CreateBookingDto newBooking)
    {
        var response = await _httpClient.PostAsJsonAsync("bookings", newBooking);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateBookingAsync(int id, EditBookingDto editBooking)
    {
        var response = await _httpClient.PutAsJsonAsync($"bookings/{id}", editBooking);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteBookingAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"bookings/{id}");
        return response.IsSuccessStatusCode;
    }
}