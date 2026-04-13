using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using npost.Core.Auth.DTO;
using npost.Core.Auth.Service;
using npost.Data;

namespace npost.Core.Auth.Controller;

[ApiController]
[Route("v1/user/")]
public class UserController(UserService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] UsuarioInputDTO model)
    {
        await service.CreateAsyc(model);
        return Ok(new EndPointResponse { });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginInputDTO model)
    {
        var result = await service.LoginAsync(model);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] UserRefreshInputDTO model)
    {
        var result = await service.RefreshTokenAsync(model);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await service.LogoutAsync();
        return Ok(new EndPointResponse { });
    }

    [Authorize]
    [HttpPut("preferences/theme")]
    public async Task<IActionResult> UpdateThemePreferenceAsync([FromBody] UserThemePreferenceInputDTO model)
    {
        var result = await service.UpdateThemePreferenceAsync(model);
        return Ok(result);
    }
}
