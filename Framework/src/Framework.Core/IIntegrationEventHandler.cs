namespace Framework.Core;

public interface IIntegrationEventHandler<T> where T : IEvent
{
    Task Handle(T @event, CancellationToken cancellationToken = default);
}
