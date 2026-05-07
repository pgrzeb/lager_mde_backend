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

        // Diese Schleife liest einen Job nach dem anderen aus
        await foreach (var job in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                // Hier erfolgt der EINE Aufruf zur Xbase API
                var response = await client.GetAsync($"https://deine-xbase-api/artikel/{job.ArtikelId}");
                var content = await response.Content.ReadAsStringAsync();
                
                // Ergebnis an den wartenden Controller zurückgeben
                job.tcs.SetResult(content);
            }
            catch (Exception ex)
            {
                job.tcs.SetException(ex);
            }
        }
    }
}