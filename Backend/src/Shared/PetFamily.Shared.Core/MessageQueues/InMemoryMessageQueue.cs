using System.Threading.Channels;
using PetFamily.Shared.Core.Messaging;

namespace PetFamily.Shared.Core.MessageQueues;

public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
{
    private readonly Channel<TMessage> _channel = Channel.CreateUnbounded<TMessage>();

    public async Task WriteAsync(TMessage files, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(files, ct);
    }

    public async Task<TMessage> ReadAsync(CancellationToken ct = default)
    {
        return await _channel.Reader.ReadAsync(ct);
    }
}