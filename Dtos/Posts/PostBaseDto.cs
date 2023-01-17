using System.ComponentModel.DataAnnotations;

namespace CSBlog.Dtos.Posts;

public class PostBaseDto
{
    [StringLength(
        100,
        MinimumLength = 20,
        ErrorMessage = "O título deve ter entre {2} a {1} caracter"
    )]
    public string Title { get; set; }

    [Required]
    public string Tags { get; set; }

    [MinLength(100, ErrorMessage = "o conteúdo deve ter no mínimo {1}")]
    public string Content { get; set; }
#nullable enable
    public string? Subtitle { get; set; }
    public string? Status { get; set; }
}
