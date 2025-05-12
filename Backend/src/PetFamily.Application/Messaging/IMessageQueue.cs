namespace PetFamily.Application.Messaging;

public interface IMessageQueue<TMessage>
{
    Task WriteAsync(TMessage files, CancellationToken ct = default);

    Task<TMessage> ReadAsync(CancellationToken ct = default);
}