using CSBlog.Data;
using CSBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Repositories;

public class UserRepository
{
    private readonly Context _context;

    public UserRepository([FromServices] Context context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User newUser)
    {
        newUser.Create();
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<List<User>> ListAsync()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        return users;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User> GetById(int id, bool tracking = true)
    {
        var action = tracking ? _context.Users : _context.Users.AsNoTracking();
        var user = await action.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }
}
