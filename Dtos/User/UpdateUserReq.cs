using System.ComponentModel.DataAnnotations;

namespace CSBlog.Dtos.User;

public class UpdateUserReq
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
}
