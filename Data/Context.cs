using CSBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    //Entities
    public DbSet<User> Users { get; set; }
}
