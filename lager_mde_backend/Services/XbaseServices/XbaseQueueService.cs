using System.Threading.Channels;

public class XbaseQueueService
{
    private readonly Channel<XbaseRequestJob> _channel;

    public XbaseQueueService()
    {
        // Unbounded oder Bounded (z.B. max 1000 Jobs in der Warteschlange)
        _channel = Channel.CreateUnbounded<XbaseRequestJob>();
    }

    public async Task<string> EnqueueJobAsync(int id)
    {
        var job = new XbaseRequestJob { ArtikelId = id };
        await _channel.Writer.WriteAsync(job);
        
        // Der Controller wartet hier, bis der Background-Service tcs.SetResult() aufruft
        return await job.tcs.Task;
    }

    public ChannelReader<XbaseRequestJob> Reader => _channel.Reader;
}