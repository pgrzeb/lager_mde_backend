using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("lagstamm")]
[ApiController]
public class LagstammController : ControllerBase 
{
    private readonly ILagstammService _lagstammlService;

    public LagstammController(ILagstammService lagstammlService)
    {
        _lagstammlService = lagstammlService;
    }

    [HttpPut("lagerplatz")]
    public async Task<ActionResult<UpdateLagstammPlatzResponse>> GetLagstammPlatzResponseAsync(UpdateLagstammPlatzRequest request, String lagerplatz)
    {
        var response = await _lagstammlService.LagstammPlatzResponseAsync(request, lagerplatz);

        return Ok(response);
    }
    
    [HttpGet("getLagstammArt")]
    public async Task<ActionResult<LagstammArtikelResponse>> GetLagstammArtikel(int artnr)
    {
        var artikel = await _lagstammlService.GetLagstammArtikelAsync(artnr);

        return Ok(artikel);
    }

}