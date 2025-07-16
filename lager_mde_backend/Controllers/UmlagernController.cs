using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("umlagern")]
[ApiController]
public class UmlagernController : ControllerBase 
{
    private readonly IUmlagernService _umlagService;

    public UmlagernController(IUmlagernService umlagService)
    {
        _umlagService = umlagService;
    }

    [HttpPut("getLagPlatzId")]
    public async Task<ActionResult<GetLagPlatzIdResponse>> GetLagPlatzId(GetLagPlatzIdRequest request)
    {
        var response = await _umlagService.GetLagPlatzIdAsync(request);

        return Ok(response);
    }

    [HttpPut("updateArtLagPlatz")]
    public async Task<ActionResult<UpdateLagPlatzResponse>> UpdateArtLagPlatz(UpdateLagPlatzRequest request)
    {
        var response = await _umlagService.UpdateArtLagPlatzAsync(request);

        return Ok(response);
    }

    [HttpGet("lagplatzVon")]
    public async Task<ActionResult<GetLagPlatzVonResponse>> GetLagPlatzVon(int id)
    {
        var response = await _umlagService.GetLagPlatzVonAsync(id);

        return Ok(response);
    }
}