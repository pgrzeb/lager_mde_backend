namespace lager_mde_backend.Models;

//Für Zeile 236 in lagertablett.prg
public class GetStapaufResponse
{
    public int stap_id { get; set; }
    public int artnr { get; set; }
    public required string artbez { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public int menge { get; set; }
    public int typ { get; set; }
    public int status { get; set; }
    public int durchl { get; set; }
    public int restmeng { get; set; } 
    public int mhd { get; set; }
}