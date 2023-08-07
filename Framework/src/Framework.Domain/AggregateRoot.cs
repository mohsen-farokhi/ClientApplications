using Framework.Core;

namespace Framework.Domain;

public abstract class AggregateRoot<TKey> : 
    Entity<TKey>, IAggregateRoot
{
    private List<IEvent> _uncommittedEvents = new();

    public IReadOnlyCollection<IEvent> UncommittedEvents() => _uncommittedEvents;

    protected void Causes(IEvent @event)
    {
        _uncommittedEvents.Add(@event);
    }
    public void ClearDomainEvents()
    {
        _uncommittedEvents.Clear();
    }
}
