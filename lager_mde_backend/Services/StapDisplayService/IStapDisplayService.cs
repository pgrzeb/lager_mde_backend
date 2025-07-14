using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IStapDisplayService
{
   Task<GetKomPlatzResponse> GetKomPlatz(GetKomPlatzRequest request);
    Task<GetLagPlatzResponse> GetLagLp(GetLagPlatzRequest request);
    Task<UpdateArtStapDisplayResponse> UpdateArtStapDisplayAsync(UpdateArtStapDisplayRequest request);
}