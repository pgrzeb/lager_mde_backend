namespace lager_mde_backend.Entities;

public class Stapauf {
    public int stap_id { get; set; }	
    public int artnr { get; set; }
    public required string artbez { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public int menge { get; set; }
    public required string datum { get; set; }
    public int typ { get; set; }
    public int status { get; set; }
    public int durchl { get; set; }
    public int restmeng { get; set; } 
    public required string staufdat { get; set; }
    public required string staufanf { get; set; }
    public required string staufend { get; set; }
    public int benutzer { get; set; }
    public int user { get; set; }
    public int mhd { get; set; }
    public DateTime? mhdatum { get; set; }
}