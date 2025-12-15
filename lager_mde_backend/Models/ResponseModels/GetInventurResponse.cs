namespace lager_mde_backend.Models;

public class GetInventurResponse {

    public required int artnr {get; set;}
    public required string artbez {get; set;}
    public required int menge {get; set;}
    public required DateOnly datum {get; set;} 
    public required int benutzer {get; set;}
    public string? nachricht { get; set; }
}