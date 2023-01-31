using CSBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    //Entities
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(p => p.UserType).HasDefaultValue(UserType.READER);
        modelBuilder.Entity<Post>().Property(p => p.Status).HasDefaultValue(PostStatus.DRAFT);
        modelBuilder
            .Entity<User>()
            .Property(p => p.UserType)
            .HasConversion(ut => ut.ToString(), ut => (UserType)Enum.Parse(typeof(UserType), ut));
        modelBuilder
            .Entity<Post>()
            .Property(p => p.Status)
            .HasConversion(
                ps => ps.ToString(),
                ps => (PostStatus)Enum.Parse(typeof(PostStatus), ps)
            );
        modelBuilder
            .Entity<User>()
            .HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    Password = "123456",
                    UserType = UserType.ADMIN
                }
            );
    }
}
