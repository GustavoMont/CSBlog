using System.ComponentModel.DataAnnotations;
using CSBlog.Models;

namespace CSBlog.Dtos.User;

public class CreateTeamUser : CreateUserRequest
{
    [Required]
    [MinLength(3, ErrorMessage = "O tipo de usuário deve ter no mínimo {1} caracteres")]
    public string UserType { get; set; }
}
