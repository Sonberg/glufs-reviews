namespace Glufs.Reviews.Domain.Events;

public interface IEventPublisher
{
    Task Publish<T>(T @event, CancellationToken cancellationToken = default) where T : Event;

    Task Publish<T>(T @event, DateTimeOffset deliversAt, CancellationToken cancellationToken = default) where T : Event;
}