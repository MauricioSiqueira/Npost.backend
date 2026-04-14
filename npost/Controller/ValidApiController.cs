using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace npost.Controller;

[ApiController]
[Route("v1/")]
public class ValidApiController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("teste")]
    public IActionResult TesteOk()
    {
        return Ok("Funcionando");
    }
}