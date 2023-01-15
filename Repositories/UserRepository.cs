using CSBlog.Data;
using Microsoft.AspNetCore.Mvc;

namespace CSBlog.Repositories;

public class UserRepository
{
    private readonly Context _context;

    public UserRepository([FromServices] Context context)
    {
        _context = context;
    }

}
