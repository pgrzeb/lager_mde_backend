namespace lager_mde_backend.Models;
public class LoginResponse
{
    public int komnr { get; set; }
    public string? komna { get; set; }
    public string? pin { get; set; }
    public int rechte { get; set; }
    public string? nachricht { get; set; }
}