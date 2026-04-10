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
}