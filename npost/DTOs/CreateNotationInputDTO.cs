using System.ComponentModel.DataAnnotations;

namespace npost.DTOs;

public class CreateNotationInputDTO
{
    [Required]
    [StringLength(70)]
    public string? Title { get; set; }

    public string? Content { get; set; }
}
