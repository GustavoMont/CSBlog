using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CSBlog.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "text")]
    public string Content { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    public int PostId { get; set; }
    public Post Post { get; set; }
}
