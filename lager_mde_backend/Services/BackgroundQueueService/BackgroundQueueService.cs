
using System.Threading.Channels;

namespace lager_mde_backend.Services;
public class BackgroundQueueService<T>
{
    private readonly Channel<T> channel = Channel.CreateBounded<T>(
        new BoundedChannelOptions(capacity: 1000)
        {
            SingleReader = false,
            SingleWriter = true,
            FullMode = BoundedChannelFullMode.Wait
        });

    public bool Enqueue (T item)
    {
        return channel.Writer.TryWrite(item);
    }

    public ValueTask<T> DequeueAsync(CancellationToken token = default)
    {
        return channel.Reader.ReadAsync(token);
    }

    public ValueTask<bool> WaitForTheNextReadAsync(CancellationToken token = default)
    {
        return channel.Reader.WaitToReadAsync(token);
    }

}