using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using npost.Models;
namespace npost.Core.Auth.Model;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? UsuarioId { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(30, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
    public string? Nome { get; set; }
    
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(30, ErrorMessage = "O Sobrenome deve ter no máximo {1} caracteres.")]
    public string? Sobrenome { get; set; }

    [Required]
    [StringLength(64)]
    public string? Senha { get; set; }

    [Required]
    [StringLength(70)]
    public string? Email { get; set; }

    [Required]
    [StringLength(11)]
    public string? Telefone { get; set; }
    
    [Required]
    [Column(TypeName = "date")]
    public DateTime DtNascimento { get; set; }
    
    [StringLength(11)] 
    public bool DarkMode { get; set; } = false;

    public virtual ICollection<Notation>? Notations { get; set; }
}
