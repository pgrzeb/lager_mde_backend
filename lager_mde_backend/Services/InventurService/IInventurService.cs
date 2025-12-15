using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IInventurService
{
    Task<GetInvArtResponse> GetArtAsync(int artnr);
    Task<InventurResponse> SaveInventurAsync(UpdateInventurRequest request);
    Task<InventurResponse> UpdateMengeAsync(UpdateInventurRequest request);
    Task<List<GetInventurResponse>> GetInventurAsync();
}