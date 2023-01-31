using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSBlog.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Title { get; set; }

#nullable enable
    [Column(TypeName = "varchar(150)")]
    public string? Subtitle { get; set; }

#nullable disable
    [Required]
    [Column(TypeName = "longtext")]
    public string Content { get; set; }

    [Required]
    public PostStatus Status { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Tags { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [Required]
    public int AuthorId { get; set; }
    public User Author { get; set; }

    public List<Comment> Comments { get; set; }

    public void Create()
    {
        CreatedAt = DateTime.Now.Date;
        UpdatedAt = DateTime.Now.Date;
    }

    public void Update()
    {
        UpdatedAt = DateTime.Now.Date;
    }
}
