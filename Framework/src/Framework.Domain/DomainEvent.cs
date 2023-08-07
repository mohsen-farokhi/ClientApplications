using Framework.Core;

namespace Framework.Domain
{
    public abstract class DomainEvent : IEvent
    {
        public DomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public DateTime CreatedDate { get; private set; }
        public Guid EventId { get; private set; }
    }
}