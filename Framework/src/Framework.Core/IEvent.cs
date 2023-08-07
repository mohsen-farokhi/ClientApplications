namespace Framework.Core;

public interface IEvent
{
    public DateTime CreatedDate { get; }
    public Guid EventId { get; }
}
