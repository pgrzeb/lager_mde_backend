namespace lager_mde_backend.Models;

public class UpdateArtStapDisplayRequest
{
    public int stap_id { get; set; }
    public int artnr { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public int menge { get; set; }
    public int typ { get; set; }
    public int durchl { get; set; }
    public int restmeng { get; set; }
    public int benutzer { get; set; }
    public required string lagerplatz { get; set; }
}