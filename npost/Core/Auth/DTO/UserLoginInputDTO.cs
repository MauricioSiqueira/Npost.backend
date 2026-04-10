using System.ComponentModel.DataAnnotations;

namespace npost.Core.Auth.DTO;

public class UserLoginInputDTO
{
    [Required(ErrorMessage = "Informar o email")]
    [EmailAddress]
    [StringLength(70, ErrorMessage = "Email deve conter no máximo 70 caracteres.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Informar Senha.")]
    [StringLength(16, MinimumLength = 8, ErrorMessage = "A senha deve conter no mínimo 8 caracteres e no máximo 16 caracteres.")]
    public string? password { get; set; }
}
