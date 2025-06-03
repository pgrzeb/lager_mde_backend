namespace lager_mde_backend.Models;
public class LoginRequest
{
    public int komnr { get; set; }
    public required string pin { get; set; }
}