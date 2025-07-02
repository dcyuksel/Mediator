using Mediator.Contracts;

namespace Mediator;
public interface IPublisher
{
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
}
