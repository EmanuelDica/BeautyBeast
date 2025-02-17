namespace BeautyBeastFrontend.Dtos;

public record LoginRequestDto(string Email, string Password);

public record LoginResponseDto(string Token, string UserName);