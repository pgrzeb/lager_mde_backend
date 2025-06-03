using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IStapaufService
{
    Task<List<StapDisplayResponse>> GetStapDisplayAsync(int stapStatus);
    Task<GetStapaufResponse> GetStapaufAsync(int stapId);
    Task<UpdateStapMengeResponse> UpdateStapMengeAsync(UpdateStapMengeRequest request);
    Task<UpdateStapaufResponse> UpdateStapMhdAsync(int stapId, DateTime mhdatum);
    Task<UpdateStapaufResponse> UpdateStapaufAsync(UpdateStapaufRequest request);
}