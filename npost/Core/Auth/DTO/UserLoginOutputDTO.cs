namespace npost.Core.Auth.DTO;

public class UserLoginOutputDTO
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool DarkMode { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}
