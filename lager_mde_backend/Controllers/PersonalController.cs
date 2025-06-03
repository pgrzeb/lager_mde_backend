using Microsoft.AspNetCore.Mvc;
using lager_mde_backend.Models;
using lager_mde_backend.Services;


namespace lager_mde_backend.Controllers;

[Route("personal")]
[ApiController]
public class PersonalController : ControllerBase 
{
    private readonly IPersonalService _personalService;

    public PersonalController(IPersonalService personalService)
    {
        _personalService = personalService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _personalService.LoginAsync(request);

            if (response.nachricht == "Ungültige komnr oder pin.")
            {
                return Unauthorized(response.nachricht);
            }

            return Ok(response);
    }
    

}