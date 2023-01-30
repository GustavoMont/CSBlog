namespace CSBlog.Dtos.Email;

public class SendEmailReq
{
    public string Subject { get; set; }
    public string FullName { get; set; }
    public string Content { get; set; }
    public string Email { get; set; }
}
