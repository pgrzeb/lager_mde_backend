using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IArtikelService
{
    Task<GetArtikelResponse> GetArtikelAsync(int artnr);
}