namespace BeautyBeastServer.Services;
using System.Net.Http.Json;
using BeautyBeastServer.Dtos;

public class PostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PostDto>> GetAllPostsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<PostDto>>("posts");
        return result ?? new List<PostDto>();
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<PostDto?>($"posts/{id}");
    }

    public async Task<bool> CreatePostAsync(CreatePostDto newPost)
    {
        var response = await _httpClient.PostAsJsonAsync("posts", newPost);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePostAsync(int id, EditPostDto editPost)
    {
        var response = await _httpClient.PutAsJsonAsync($"posts/{id}", editPost);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"posts/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<PostDto>> GetPostsByUserIdAsync(int userId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<PostDto>>($"posts/user/{userId}");
        return response ?? new List<PostDto>();
    }
}