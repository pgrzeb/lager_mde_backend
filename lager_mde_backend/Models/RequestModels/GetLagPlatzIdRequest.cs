namespace lager_mde_backend.Models;

public class GetLagPlatzIdRequest
{
    public bool vonLag { get; set; }
    public required string lagerplatz { get; set; }
}