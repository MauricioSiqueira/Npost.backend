using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using npost.Core;

namespace npost.Service;

public class TokenService
{
     private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int GetUsuario()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.Claims!.First(x => x.Type == Constants.ClaimUsuario).Value);
    }    

    public static string Generation(int usuarioId, int tokenExpire, bool bearer = true)
    {
        var claims = new List<Claim>
        {
            new Claim(Constants.ClaimUsuario, usuarioId.ToString()),            
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.Now.AddMinutes(tokenExpire),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Secret.GetJWTEncodedSecretKeyToken()),
            SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return bearer ? "Bearer " + tokenHandler.WriteToken(token) : tokenHandler.WriteToken(token);
    }

    public string RefreshToken()
    {
        var claims = _httpContextAccessor.HttpContext!.User.Claims;
        var usuarioId = int.Parse(claims!.First(x => x.Type == Constants.ClaimUsuario).Value);

        return Generation( usuarioId, Constants.TokenExpire);
    }
}