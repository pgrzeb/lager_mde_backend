using System.Text;
using System.Text.Json;

namespace lager_mde_backend.Services;
public class XBaseService : IXBaseService
{
    private readonly SemaphoreSlim _limiter = new(2,2);
    private readonly HttpClient _client;

    public XBaseService(HttpClient client)
    {
        _client = client;
    }

    public async Task<T> GetAsync<T>(string url)
    {
        await _limiter.WaitAsync();

        try
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json)!;
        }
        finally
        {
            _limiter.Release();
        }
    }

    public async Task<TResponse> PostAsync<TResponse>( string url, object request)
    {
        await _limiter.WaitAsync();

        try
        {
            var json = JsonSerializer.Serialize(request);

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TResponse>(jsonResponse)
                ?? throw new Exception("Ungültige Antwort von XBase.");
        }
        finally
        {
            _limiter.Release();
        }
    }
}