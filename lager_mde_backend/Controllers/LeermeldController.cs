using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("leermeld")]
[ApiController]
public class LeermeldController : ControllerBase 
{
    private readonly ILeermeldService _leermeldService;

    public LeermeldController(ILeermeldService leermeldService)
    {
        _leermeldService = leermeldService;
    }

    [HttpGet("getLeermeldArt")]
    public async Task<ActionResult<LeermeldArtResponse>> GetLeermeldArt(int artnr)
    {
        var response = await _leermeldService.GetLeermeldAsync(artnr);

        return Ok(response);
    }

    [HttpPut("updateLeermeld")]
    public async Task<ActionResult<UpdateLeermeldResponse>> UpdateLeermeld(UpdateLeermeldRequest request)
    {
        var response = await _leermeldService.UpdateLeermeldAsync(request);

        return Ok(response);
    }

}