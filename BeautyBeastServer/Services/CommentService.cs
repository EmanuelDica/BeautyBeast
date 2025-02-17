namespace BeautyBeastServer.Services;
using BeautyBeastServer.Dtos;
using System.Net.Http.Json;

public class CommentService
{
    private readonly HttpClient _httpClient;

    public CommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CommentDto>> GetAllCommentsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<CommentDto>>("comments");
        return result ?? new List<CommentDto>();
    }

    public async Task<CommentDto?> GetCommentByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<CommentDto?>($"comments/{id}");
    }

    public async Task<bool> CreateCommentAsync(CreateCommentDto newComment)
    {
        var response = await _httpClient.PostAsJsonAsync("comments", newComment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCommentAsync(int id, EditCommentDto editComment)
    {
        var response = await _httpClient.PutAsJsonAsync($"comments/{id}", editComment);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCommentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"comments/{id}");
        return response.IsSuccessStatusCode;
    }
}