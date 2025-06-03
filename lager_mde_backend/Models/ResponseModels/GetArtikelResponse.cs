namespace lager_mde_backend.Models;

public class GetArtikelResponse {
    public int? art_id { get; set; }	
    public int? artnr { get; set; }
    public string? artbez { get; set; }
    public string? lagerplatz { get; set; }
    public int? anzahl_pal { get; set; }
    public int? block { get; set; }
    public int? durchl { get; set; }
    public required string nachricht { get; set; }
}