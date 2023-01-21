namespace CSBlog.Dtos.Posts;

using CSBlog.Dtos.User;
using CSBlog.Models;

public class PostRes : PostBaseDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public UserResponse Author { get; set; }
}
