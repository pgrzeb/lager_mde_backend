namespace lager_mde_backend.Models;

public class UpdateInventurRequest {
    public int artnr { get; set; }
    public string? artbez { get; set; }
    public int menge { get; set; }
    public int benutzer { get; set; }
}