using System.ComponentModel.DataAnnotations;

namespace npost.Core.Auth.DTO;

public class UsuarioInputDTO
{
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(30, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
    public string? Nome { get; set; }
    
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(30, ErrorMessage = "O Sobrenome deve ter no máximo {1} caracteres.")]
    public string? Sobrenome { get; set; }

    [Required(ErrorMessage =  "Informar Senha.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])\S{8,16}$", ErrorMessage = "Senha deve conter ao menos Um caractere maiúsculo, um minúsculo, um número, um especial, mínimo 8 caracteres e máximo de 16.")]
    [StringLength(16, MinimumLength = 8, ErrorMessage =  "A senha deve conter no mínimo 8 caracteres e no máximo 16 caracteres.")]
    public string? Senha { get; set; }
    
    [Required(ErrorMessage =  "Informar Senha.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])\S{8,16}$", ErrorMessage = "Senha deve conter ao menos Um caractere maiúsculo, um minúsculo, um número, um especial, mínimo 8 caracteres e máximo de 16.")]
    [StringLength(16, MinimumLength = 8, ErrorMessage =  "A senha deve conter no mínimo 8 caracteres e no máximo 16 caracteres.")]
    public string? ConfirmacaoSenha { get; set; }

    [Required(ErrorMessage = "Informar o email")]
    [EmailAddress]
    [StringLength(70, ErrorMessage = "Email deve conter no máximo 70 caracteres.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Informar o celular")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Telefone01 deve conter apenas valores numéricos")]
    [Phone]
    [StringLength(11)]
    public string? Celular { get; set; }
    
    [Required(ErrorMessage = "Informar a Data de Nascimento.")]
    public DateTime DataDeNascimento { get; set; }
}