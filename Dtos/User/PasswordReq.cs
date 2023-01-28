using System.ComponentModel.DataAnnotations;

namespace CSBlog.Dtos.User;

public class PasswordReq
{
    [MinLength(8, ErrorMessage = "A senha deve ter o mínimo {1} caracteres")]
    [RegularExpression(
        "^(?=.*[A-Za-z])(?=.*?[0-9]).{8,}$",
        ErrorMessage = "A senha deve ter no mínimo uma letra e um número"
    )]
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
