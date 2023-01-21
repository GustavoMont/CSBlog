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

    public async Task<List<Post>> ListDraftAsync()
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.DRAFT)
            .Include(p => p.Author)
            .ToListAsync();
        return posts;
    }

    public async Task<List<Post>> ListPublishedAsync()
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.PUBLISHED)
            .Include(p => p.Author)
            .ToListAsync();
        return posts;
    }

    public async Task<List<Post>> ListAsync(int? id)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.DRAFT ? id == null || p.AuthorId == id : true)
            .Include(p => p.Author)
            .ToListAsync();
        return posts;
    }

    public async Task<Post> GetOneAsync(int id, bool tracking)
    {
        var post = tracking
            ? await _context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id)
            : await _context.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
        return post;
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Post post)
    {
        _context.Remove(post);
        await _context.SaveChangesAsync();
    }
}
