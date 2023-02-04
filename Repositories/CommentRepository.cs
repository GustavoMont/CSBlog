using CSBlog.Data;
using CSBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Repositories;

public class CommentRepository
{
    private readonly Context _context;

    public CommentRepository([FromServices] Context context)
    {
        _context = context;
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _context.Comments.CountAsync();
        return count;
    }

    public async Task<Comment> CreateAsync(Comment newComment)
    {
        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
        return newComment;
    }

    public async Task<List<Comment>> GetPostCommentsAsync(int postId, int skip = 0, int take = 10)
    {
        var comments = await _context.Comments
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .Include(c => c.User)
            .Where(c => c.PostId == postId)
            .ToListAsync();
        return comments;
    }

    public async Task<Comment> GetOneAsync(int id, bool tracking = true)
    {
        var action = tracking
            ? _context.Comments.Include(c => c.User)
            : _context.Comments.Include(c => c.User).AsNoTracking();
        var comment = await action.FirstOrDefaultAsync(c => c.Id == id);
        return comment;
    }

    public async Task DeleteAsync(Comment comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
