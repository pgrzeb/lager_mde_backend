namespace lager_mde_backend.Models;

public class GetLagPlatzVonResponse
{
    public int artnr { get; set; }
    public required string artbez { get; set; }
    public int menge { get; set; }

    public int tag { get; set; }
    public int monat { get; set; }
    public int jahr { get; set; }
    public required string zeit { get; set; }
    public DateTime mhdatum { get; set; }
}