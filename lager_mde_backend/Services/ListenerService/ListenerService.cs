using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text.Json;

public class ListenerService : BackgroundService
{
    private readonly IHubContext<DataHub> _hubContext;
    private readonly IConfiguration _config;

    public ListenerService(IHubContext<DataHub> hubContext, IConfiguration config)
    {
        _hubContext = hubContext;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connString = _config.GetConnectionString("DefaultConnection");

        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync(stoppingToken);

        conn.Notification += async (o, e) =>
        {
            try
            {
                var payload = JsonDocument.Parse(e.Payload).RootElement;

                var stapId = payload.GetProperty("stap_id").GetInt32();
                var op = payload.GetProperty("operation").GetString();

                // Push an SignalR senden
                await _hubContext.Clients.All.SendAsync("ReceiveDataUpdate", new {
                    stap_id = stapId,
                    operation = op
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Verarbeiten der Datenbankbenachrichtigung", ex);
            }
        };

        using var listenCmd = conn.CreateCommand();
        listenCmd.CommandText = "LISTEN stapauf_changed;";
        await listenCmd.ExecuteNonQueryAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await conn.WaitAsync(stoppingToken);
        }
    }
}
