using Framework.Core;

namespace Framework.Domain;

public interface IAggregateRoot
{
    IReadOnlyCollection<IEvent> UncommittedEvents();
    void ClearDomainEvents();
}