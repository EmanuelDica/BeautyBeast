namespace BeautyBeastApi.Dtos;
public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required string UserName { get; set; } 
}