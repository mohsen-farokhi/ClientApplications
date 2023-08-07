namespace Framework.Core;

public interface IDomainEventHandler<T> where T : IEvent
{
    Task Handle(T @event, CancellationToken cancellationToken = default);
}
