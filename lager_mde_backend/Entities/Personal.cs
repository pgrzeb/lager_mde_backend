namespace lager_mde_backend.Entities;

public class Personal {
    public int pers_id { get; set; }
    public int komnr { get; set; }
    public required string komna { get; set; }
    public required string pin { get; set; }
    public int rechte { get; set; }
}