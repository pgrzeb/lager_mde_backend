namespace lager_mde_backend.Models;

//Für Zeile 109 in lagertablett.prg
public class StapDisplayResponse
{
    public int stap_id { get; set; }
    public required string artbez { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public int menge { get; set; }
    public int typ { get; set; }
    public int status { get; set; }

}