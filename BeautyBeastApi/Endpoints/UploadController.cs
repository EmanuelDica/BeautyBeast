using Microsoft.AspNetCore.Mvc;

namespace BeautyBeastApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MediaController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided or the file is empty.");

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            Directory.CreateDirectory(uploadPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/Uploads/{uniqueFileName}";
            return Ok(new { Path = relativePath }); // Return the relative path for the frontend
        }
    }
}