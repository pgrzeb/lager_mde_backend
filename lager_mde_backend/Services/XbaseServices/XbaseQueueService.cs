using System.Threading.Channels;
using lager_mde_backend.Jobs;
using lager_mde_backend.Models;

namespace lager_mde_backend.Services;
public class XbaseQueueService
{
    private readonly Channel<XbaseRequestJob> _channel;

    public XbaseQueueService()
    {
        // Unbounded oder Bounded (z.B. max 1000 Jobs in der Warteschlange)
        _channel = Channel.CreateUnbounded<XbaseRequestJob>();
    }

    public async Task<GetInvArtResponse> EnqueueJobAsync(int id)
{
    var job = new XbaseRequestJob { ArtikelId = id };
    await _channel.Writer.WriteAsync(job);
    return await job.tcs.Task; // Wartet, bis der Worker das Objekt liefert
}

    public ChannelReader<XbaseRequestJob> Reader => _channel.Reader;
}