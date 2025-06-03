namespace lager_mde_backend.Models;

public class UpdateLeermeldRequest
{
    public int? lag_id { get; set; }
    public int artnr { get; set; }
    public required string artbez { get; set; }
    public required string ziel { get; set; }
    public required string von { get; set; }
    public int menge { get; set; }
    public int durchl { get; set; }
    public int komnr {get; set; }
}