using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("stapauf")]
[ApiController]
public class StapaufController : ControllerBase 
{
    private readonly IStapaufService _stapaufService;

    public StapaufController(IStapaufService stapaufService)
    {
        _stapaufService = stapaufService;
    }

    [HttpGet("stapDisplay")]
    public async Task<ActionResult<StapDisplayResponse>> GetStapDisplay(int stapStatus)
    {
        var response = await _stapaufService.GetStapDisplayAsync(stapStatus);

        return Ok(response);
    }

    [HttpGet("{stapId}")]
    public async Task<ActionResult<GetStapaufResponse>> GetStapauf(int stapId)
    {
        var response = await _stapaufService.GetStapaufAsync(stapId);

        return Ok(response);
    }

    [HttpPut("updateStapMenge")]
    public async Task<ActionResult<UpdateStapMengeResponse>> UpdateStapMenge(UpdateStapMengeRequest request)
    {
        var response = await _stapaufService.UpdateStapMengeAsync(request);

        return Ok(response);
    }

    [HttpPut("mhdatum")]
    public async Task<ActionResult<UpdateStapaufResponse>> UpdateStapMhd(UpdateStapaufMhdRequest request, DateTime mhdatum)
    {
        var response = await _stapaufService.UpdateStapMhdAsync(request, mhdatum);

        return Ok(response);
    }

}