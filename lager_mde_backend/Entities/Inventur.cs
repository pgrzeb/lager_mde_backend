namespace lager_mde_backend.Entities;

public class Inventur {
    public int id { get; set; }
    public int artnr { get; set; }
    public string? artbez { get; set; }
    public int menge { get; set; }
    public DateOnly datum { get; set; }
    public int benutzer { get; set; }
}