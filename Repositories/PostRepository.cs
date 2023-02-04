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

    public async Task<int> GetCountAsync()
    {
        var count = await _context.Posts.CountAsync();
        return count;
    }

    public async Task<int> GetCountByStatusAsync(PostStatus status)
    {
        var count = await _context.Posts.Where(p => p.Status == status).CountAsync();
        return count;
    }

    public async Task<Post> CreateAsync(Post newPost)
    {
        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();
        return newPost;
    }

    public async Task<List<Post>> ListDraftAsync(int skip = 0, int take = 25)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.DRAFT)
            .OrderByDescending(p => p.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .Include(p => p.Author)
            .ToListAsync();
        return posts;
    }

    public async Task<List<Post>> ListPublishedAsync(int skip = 0, int take = 25)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.Status == PostStatus.PUBLISHED)
            .OrderByDescending(p => p.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .Include(p => p.Author)
            .ToListAsync();
        return posts;
    }

    public async Task<List<Post>> ListAsync(int? userId, int skip = 0, int take = 25)
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(
                p => p.Status == PostStatus.DRAFT ? userId == null || p.AuthorId == userId : true
            )
            .OrderByDescending(p => p.UpdatedAt)
            .Skip(skip)
            .Take(take)
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
