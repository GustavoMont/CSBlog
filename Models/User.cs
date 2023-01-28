using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(100)")]
    [Required]
    public string Name { get; set; }

    [Column(TypeName = "varchar(100)")]
    [Required]
    public string LastName { get; set; }

    [Column(TypeName = "varchar(150)")]
    [Required]
    public string Email { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    private string _password;
    public string Password
    {
        get { return _password; }
        set { _password = BCrypt.Net.BCrypt.HashPassword(value); }
    }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [Column(TypeName = "varchar(20)")]
    public UserType? UserType { get; set; }

    public List<Post> Posts { get; set; }

    public void Create()
    {
        CreatedAt = DateTime.Now.Date;
        UpdatedAt = DateTime.Now.Date;
    }
}
