using System.ComponentModel.DataAnnotations;

namespace BeautyBeast.Frontend.Dtos;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required string UserName { get; set; } 
    public required string Role { get; set; }
    public required int UserId { get; set; } 
}