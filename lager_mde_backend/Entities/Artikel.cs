namespace lager_mde_backend.Entities;

public class Artikel {
    public int art_id { get; set; }	
    public int artnr { get; set; }
    public required string artbez { get; set; }
    public required string lagerplatz { get; set; }
    public int anzahl_pal { get; set; }
    public int block { get; set; }
    public int durchl { get; set; }
}