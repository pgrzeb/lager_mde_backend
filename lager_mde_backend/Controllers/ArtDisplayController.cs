using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("art")]
[ApiController]
public class ArtDisplayController : ControllerBase 
{
    private readonly IArtDisplayService _artService;

    public ArtDisplayController(IArtDisplayService artService)
    {
        _artService = artService;
    }

    [HttpGet("komplatz")]
    public async Task<ActionResult<GetKomPlatzResponse>> GetKomPlatz(int artnr)
    {
        var response = await _artService.GetKomPlatzAsync(artnr);

        return Ok(response);
    }

    [HttpGet("{lagerplatz}")]
    public async Task<ActionResult<GetLagPlatzResponse>> GetLagPlatz(string lagerplatz)
    {
        var response = await _artService.GetLagPlatzAsync(lagerplatz);

        return Ok(response);
    }

    [HttpPut("updateStapArt")]
    public async Task<ActionResult<UpdateArtStapDisplayResponse>> UpdateArtStapDisplay(UpdateArtStapDisplayRequest request)
    {
        var response = await _artService.UpdateArtStapDisplayAsync(request);

        return Ok(response);
    }
}