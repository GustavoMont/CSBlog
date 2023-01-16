using CSBlog.Models;

namespace CSBlog.Dtos.User;

public class UserResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; }
}
