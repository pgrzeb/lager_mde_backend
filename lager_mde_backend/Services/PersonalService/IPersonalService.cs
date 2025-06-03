using lager_mde_backend.Models;

namespace lager_mde_backend.Services;

public interface IPersonalService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}