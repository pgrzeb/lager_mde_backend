namespace lager_mde_backend.Models;

public class LeermeldArtResponse {	
    public int? lag_id { get; set; }
    public int? artnr { get; set; }
    public string? artbez { get; set; }
    public int? menge { get; set; }
    public string? von { get; set; }
    public int? durchl { get; set; }
    public string? ziel { get; set; }
    public required string nachricht { get; set; }
}