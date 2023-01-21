using CSBlog.Data;
using CSBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Repositories;

public class PostRepository
{
    private readonly Context _context;

    public PostRepository([FromServices] Context context)
    {
        _context = context;
    }

    public async Task<Post> CreateAsync(Post newPost)
    {
        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();
        return newPost;
    }

    public async Task<List<Post>> ListAsync()
    {
        var posts = await _context.Posts.AsNoTracking().Include(p => p.Author).ToListAsync();
        return posts;
    }

    public async Task<Post> GetOneAsync(int id)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        return post;
    }
}
