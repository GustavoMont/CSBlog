using System.ComponentModel.DataAnnotations;

namespace CSBlog.Dtos.User;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Nome é um campo obrigatório")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "Nome deve ter entre {2} e {1} caracteres"
    )]
    public string Name { get; set; }

    [Required(ErrorMessage = "Sobrenome é um campo obrigatório")]
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "Sobrenome deve ter entre {2} e {1} caracteres"
    )]
    public string LastName { get; set; }

    [RegularExpression(
        "^[a-z0-9.]+@[a-z0-9]+.[a-z]+.([a-z]+)?$",
        ErrorMessage = "Insira um e-mail válido"
    )]
    [Required(ErrorMessage = "E-mail é um campo obrigatório")]
    public string Email { get; set; }

    [MinLength(8, ErrorMessage = "A senha deve ter o mínimo {1} caracteres")]
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    [MinLength(3, ErrorMessage = "O tipo de usuário deve ter no mínimo {1} caracteres")]
    public string UserType { get; set; }
}
