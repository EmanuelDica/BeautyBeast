using BeautyBeastApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("hashedpasswords")]
public class DebugController : ControllerBase
{
    private readonly BeautyBeastContext _dbContext;

    public DebugController(BeautyBeastContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetHashedPasswords()
    {
        var users = await _dbContext.Users
            .Select(u => new { u.Id, u.Email, u.HashedPassword })
            .ToListAsync();

        return Ok(users);
    }
}