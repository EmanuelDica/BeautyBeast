using Microsoft.AspNetCore.Mvc;
using BeautyBeastApi.Helpers;
using Microsoft.EntityFrameworkCore;
using BeautyBeastApi.Data;
using BeautyBeastApi.Entities;
using BeautyBeastApi.Dtos;

namespace BeautyBeastApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BeautyBeastContext _dbContext;

        public AuthController(IConfiguration configuration, BeautyBeastContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var secretKey = _configuration["Jwt:Key"] ?? "default-secret-key";
            var issuer = _configuration["Jwt:Issuer"] ?? "default-issuer";

            var user = await _dbContext.Users
                .Where(u => u.Email == request.Email)
                .Select(u => new { u.Id, u.Email, u.HashedPassword, u.Role })
                .FirstOrDefaultAsync();

            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.HashedPassword))
            {
                return Unauthorized("Invalid login attempt.");
            }

            var token = JwtTokenHelper.GenerateToken(user.Email, user.Role, user.Id, secretKey, issuer);
            return Ok(new { Token = token , Role = user.Role });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _dbContext.Users.AnyAsync(u => u.Email == request.Email);
            if (existingUser)
            {
                return BadRequest("Email is already registered.");
            }

            var newUser = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                HashedPassword = PasswordHelper.HashPassword(request.Password), 
                Role = request.Role
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully." });
        }

    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}