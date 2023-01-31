using System.ComponentModel.DataAnnotations;

namespace CSBlog.Dtos.Comments;

public class CreateCommentReq
{
    [Required]
    [StringLength(700, MinimumLength = 3)]
    public string Content { get; set; }
}
