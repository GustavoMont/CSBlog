using CSBlog.Dtos.User;

namespace CSBlog.Dtos.Comments;

public class CommentRes
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public UserResponse User { get; set; }
    public int PostId { get; set; }
}
