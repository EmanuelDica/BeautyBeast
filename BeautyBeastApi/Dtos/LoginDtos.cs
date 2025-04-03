using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;
public class LoginRequest
{
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required string UserName { get; set; } 
    public required string Role { get; set; }
    public required int UserId { get; set; } 
}
