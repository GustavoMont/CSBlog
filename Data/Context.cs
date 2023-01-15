using CSBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    //Entities
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .Property(p => p.UserType)
            .HasConversion(ut => ut.ToString(), ut => (UserType)Enum.Parse(typeof(UserType), ut));
    }
}
