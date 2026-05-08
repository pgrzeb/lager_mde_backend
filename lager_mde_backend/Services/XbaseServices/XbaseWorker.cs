using System.Text.Json;
using lager_mde_backend.Models;

namespace lager_mde_backend.Services;
public class XbaseWorker : BackgroundService
{
    private readonly XbaseQueueService _queue;
    private readonly IHttpClientFactory _httpClientFactory;

    public XbaseWorker(XbaseQueueService queue, IHttpClientFactory httpClientFactory)
    {
        _queue = queue;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    var client = _httpClientFactory.CreateClient();

    await foreach (var job in _queue.Reader.ReadAllAsync(stoppingToken))
    {
        try
        {
            var response = await client.GetAsync($"http://192.168.125.111:8080/inventur/getArt?artnr={job.ArtikelId}");
            
            if (!response.IsSuccessStatusCode) {
                job.tcs.SetResult(new GetInvArtResponse { nachricht = "Xbase Fehler oder nicht gefunden" });
                continue;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var content = JsonSerializer.Deserialize<GetInvArtResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Optional: Ein kleiner Delay für Xbase++, falls die API Zeit braucht
            await Task.Delay(50); 

            // Das fertige Objekt zurück an den Controller geben
            job.tcs.SetResult(content ?? new GetInvArtResponse { nachricht = "Leere Antwort" });
        }
        catch (Exception ex)
        {
            job.tcs.SetException(ex);
        }
    }
}
}