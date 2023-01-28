namespace CSBlog.Dtos.User;

public class ResetPasswordReq : PasswordReq
{
    public string Token { get; set; }
}
