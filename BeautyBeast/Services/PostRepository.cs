using BeautyBeastApp.Data;
using BeautyBeastApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautyBeastApp.Services
{
    public class PostRepository
    {
        private readonly BeautyBeastDbContext _context;

        public PostRepository(BeautyBeastDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Artist)
                .OrderByDescending(p => p.DatePosted)
                .ToListAsync();
        }

        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }
    }
}