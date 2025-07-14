using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IStapDisplayService
{
    Task<GetKomPlatzResponse> GetKomPlatzAsync(GetKomPlatzRequest request);
    Task<GetLagPlatzResponse> GetLagLpAsync(GetLagPlatzRequest request);
    Task<UpdateArtStapDisplayResponse> UpdateArtStapDisplayAsync(UpdateArtStapDisplayRequest request, string lagerplatz);
}