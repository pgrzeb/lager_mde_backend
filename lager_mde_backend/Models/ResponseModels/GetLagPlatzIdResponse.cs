namespace lager_mde_backend.Models;

public class GetLagPlatzIdResponse
{
    public bool gefunden { get; set; }
    public int? lag_id {get; set;} 
    public string? nachricht { get; set; }
}