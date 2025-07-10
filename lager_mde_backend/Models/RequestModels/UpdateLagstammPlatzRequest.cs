using System.ComponentModel.DataAnnotations;

namespace lager_mde_backend.Models;

public class UpdateLagstammPlatzRequest
{
    public int restmeng { get; set; }
    public required string von { get; set; }
    public required string ziel { get; set; }
    public int artnr { get; set; }
    public int menge { get; set; }
    public int typ { get; set; }
}