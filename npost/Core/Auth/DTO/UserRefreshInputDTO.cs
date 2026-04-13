using System.ComponentModel.DataAnnotations;

namespace npost.Core.Auth.DTO;

public class UserRefreshInputDTO
{
    [Required(ErrorMessage = "Informar refresh token.")]
    public string? RefreshToken { get; set; }
}
