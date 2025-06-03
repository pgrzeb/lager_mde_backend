using lager_mde_backend.Entities;
using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface ILagstammService
{
    Task<UpdateLagstammPlatzResponse> LagstammPlatzResponseAsync(UpdateLagstammPlatzRequest request, String lagerplatz);
    Task<LagstammArtikelResponse> GetLagstammArtikelAsync(int artnr);
    Task<UpdateLagstammStatusResponse> UpdateLagstammStatusAsync(int lagId);
}