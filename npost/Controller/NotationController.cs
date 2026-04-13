using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using npost.Data;
using npost.DTOs;
using npost.Service;

namespace npost.Controller;

[ApiController]
[Authorize]
[Route("v1/notation")]
public class NotationController(NotationService service) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync()
    {
        var result = await service.GetListAsync();
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByTitleAsync([FromQuery] string? query)
    {
        var result = await service.SearchByTitleAsync(query);
        return Ok(result);
    }

    [HttpGet("{notationId:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid notationId)
    {
        var result = await service.GetByIdAsync(notationId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateNotationInputDTO model)
    {
        var result = await service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("{notationId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid notationId, [FromBody] UpdateNotationInputDTO model)
    {
        var result = await service.UpdateAsync(notationId, model);
        return Ok(result);
    }

    [HttpDelete("{notationId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid notationId)
    {
        await service.DeleteAsync(notationId);
        return Ok(new EndPointResponse { });
    }
}
