namespace Framework.Core;

public interface IEventBus
{
    Task Publish<T>(T @event) where T : IEvent;
}