using Microsoft.AspNetCore.Mvc;
using BeautyBeastApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BeautyBeastApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var secretKey = _configuration["Jwt:Key"] ?? "default-secret-key";
            var issuer = _configuration["Jwt:Issuer"] ?? "default-issuer";

            var user = await DbContext.Users
                .Where(u => u.Email == request.Email && u.Password == request.Password)
                .Select(u => new { u.Id, u.Email, u.Role })
                .FirstOrDefaultAsync(); 

    if (user != null)
    {
        var token = JwtTokenHelper.GenerateToken(user.Email, user.Role, user.Id, secretKey, issuer);
        return Ok(new { Token = token });
    }

            return Unauthorized("Invalid login attempt.");
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}