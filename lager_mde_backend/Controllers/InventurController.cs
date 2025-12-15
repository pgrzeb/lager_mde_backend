using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("inventur")]
[ApiController]
public class InventurController : ControllerBase 
{
    private readonly IInventurService _inventurService;

    public InventurController(IInventurService inventurService)
    {
        _inventurService = inventurService;
    }

    [HttpGet("getArt")]
    public async Task<ActionResult<GetInvArtResponse>> GetArt(int artnr)
    {
        var response = await _inventurService.GetArtAsync(artnr);

        return Ok(response);
    }

    [HttpPut("save")]
    public async Task<ActionResult<InventurResponse>> SaveInventur(UpdateInventurRequest request)
    {
        var response = await _inventurService.SaveInventurAsync(request);

        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<ActionResult<InventurResponse>> UpdateMenge(UpdateInventurRequest request)
    {
        var response = await _inventurService.UpdateMengeAsync(request);

        return Ok(response);
    }

    [HttpGet("getInv")]
    public async Task<ActionResult<InventurResponse>> GetInventur()
    {
        var response = await _inventurService.GetInventurAsync();

        return Ok(response);
    }
}