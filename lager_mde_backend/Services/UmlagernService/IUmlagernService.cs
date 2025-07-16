using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IUmlagernService
{
    Task<GetLagPlatzIdResponse> GetLagPlatzIdAsync(GetLagPlatzIdRequest request);
    Task<UpdateLagPlatzResponse> UpdateArtLagPlatzAsync(UpdateLagPlatzRequest request);
    Task<GetLagPlatzVonResponse> GetLagPlatzVonAsync(int id);
}