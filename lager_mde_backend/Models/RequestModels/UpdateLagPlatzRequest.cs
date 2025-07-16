namespace lager_mde_backend.Models;

public class UpdateLagPlatzRequest
{
    public int vonId { get; set; }
    public int nachId { get; set; }

    public int artnr { get; set; }
    public required string artbez { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public int menge { get; set; }
    public required string datum { get; set; }
    public DateTime mhdatum { get; set; }

}