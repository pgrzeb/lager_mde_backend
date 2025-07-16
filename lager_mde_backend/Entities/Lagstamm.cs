namespace lager_mde_backend.Entities;

public class Lagstamm {
    public int lag_id { get; set; }
    public int id { get; set; }
    public required string lagerplatz { get; set; }
    public int artnr { get; set; }
    public int kisten { get; set; }
    public int sperre { get; set; }
    public int jahr { get; set; }
    public int monat { get; set; }
    public int tag { get; set; }
    public required string zeit { get; set; }
    public int klpal { get; set; }
    public int palsper1 { get; set; }
    public int palsper2 { get; set; }
    public int palsper3 { get; set; }
    public int palsper4 { get; set; }
    public int palsper5 { get; set; }
    public int palsper6 { get; set; }
    public DateTime? mhdatum { get; set; }
}