namespace lager_mde_backend.Models;

public class UpdateStapaufRequest
{
    public int artnr { get; set; }
    public required string artbez { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public required int menge { get; set; }
    public required string datum { get; set; }
    public int durchl { get; set; }
    public required string staufanf { get; set; }
}