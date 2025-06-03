using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface ILeermeldService
{
    Task<LeermeldArtResponse> GetLeermeldAsync(int artnr);
    Task<UpdateLeermeldResponse> UpdateLeermeldAsync(UpdateLeermeldRequest request);

}