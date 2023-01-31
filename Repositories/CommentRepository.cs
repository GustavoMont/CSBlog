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

    public async Task<Comment> CreateAsync(Comment newComment)
    {
        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
        return newComment;
    }

    public async Task<Comment> GetOneAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        return comment;
    }
}
