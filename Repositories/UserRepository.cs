using CSBlog.Data;
using CSBlog.Models;
using Microsoft.AspNetCore.Mvc;

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
}
