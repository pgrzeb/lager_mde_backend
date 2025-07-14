using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IArtDisplayService
{
    Task<GetKomPlatzResponse> GetKomPlatzAsync(int artnr);
    Task<GetLagPlatzResponse> GetLagPlatzAsync(string lagerplatz);
    Task<UpdateArtStapDisplayResponse> UpdateArtStapDisplayAsync(UpdateArtStapDisplayRequest request);
}