using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using npost.Core.Auth.Model;

namespace npost.Models
{
    public class Notation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid NotationId { get; set; } = Guid.NewGuid();

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(70)]
        public string? Title { get; set; }

        [Column(TypeName = "text")]
        public string? Content { get; set; }

        public virtual Usuario? User { get; set; }
    }
}
