using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("artikel")]
[ApiController]
public class ArtikelController : ControllerBase 
{
    private readonly IArtikelService _artikelService;

    public ArtikelController(IArtikelService artikelService)
    {
        _artikelService = artikelService;
    }    

    [HttpGet("getArtikel")]
    public async Task<ActionResult<GetArtikelResponse>> GetArtikel(int artnr)
    {
        var artikel = await _artikelService.GetArtikelAsync(artnr);

        return Ok(artikel);
    }
}