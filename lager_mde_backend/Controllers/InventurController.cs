using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("inventur")]
[ApiController]
public class InventurController : ControllerBase 
{
    private readonly IInventurService _inventurService;
    private readonly XbaseQueueService _queue;

    public InventurController(IInventurService inventurService, XbaseQueueService queue)
    {
        _inventurService = inventurService;
        _queue = queue;
    }

    [HttpGet("getArt")]
    public async Task<ActionResult<GetInvArtResponse>> GetArt(int artnr)
    {
        try
        {
            var result = await _queue.EnqueueJobAsync(artnr);
            return Ok(result);
        } catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

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
}