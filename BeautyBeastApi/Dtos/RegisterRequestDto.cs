using System.ComponentModel.DataAnnotations;

namespace BeautyBeastApi.Dtos;

public class RegisterRequest
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string Role { get; set; } = string.Empty; 
}