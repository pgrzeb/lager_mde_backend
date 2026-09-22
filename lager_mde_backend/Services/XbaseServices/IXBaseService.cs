
namespace lager_mde_backend.Services;

public interface IXBaseService
{
    Task<T> GetAsync<T>(string url);
    Task<TResponse> PostAsync<TResponse>(string url, object request);
}