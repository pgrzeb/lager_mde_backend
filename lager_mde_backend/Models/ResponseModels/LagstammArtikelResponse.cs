namespace lager_mde_backend.Models;

public class LagstammArtikelResponse {
    public int? artnr { get; set; }
    public int? sperre { get; set; }
    public string? lagplatz { get; set; }
    public int? lagMeng { get; set; }

    public required string nachricht { get; set; }
} 