namespace CSBlog.Dtos.Posts;

using CSBlog.Models;

public class PostRes
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public User Author { get; set; }
}
