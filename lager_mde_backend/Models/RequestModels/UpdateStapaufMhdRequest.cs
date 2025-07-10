namespace lager_mde_backend.Models;

public class UpdateStapaufMhdRequest
{
    public int stap_id { get; set; }
    public required string ziel { get; set; }
}