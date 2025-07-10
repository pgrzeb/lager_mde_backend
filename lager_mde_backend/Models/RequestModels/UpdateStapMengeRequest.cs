namespace lager_mde_backend.Models;

//Für Zeile 250 in lagertablett.prg
public class UpdateStapMengeRequest
{
    public int stap_id { get; set; }
    public int menge { get; set; }
    public required string lagerplatz { get; set; }
    public int benutzer { get; set; }
}