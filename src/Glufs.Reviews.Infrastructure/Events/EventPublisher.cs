using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Glufs.Reviews.Domain.Events;

namespace Glufs.Reviews.Infrastructure.Events;


public class EventPublisher : IEventPublisher
{
    private readonly ServiceBusClient _serviceBusClient;

    public EventPublisher(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }

    public async Task Publish<T>(T @event, CancellationToken cancellationToken = default) where T : Event
    {
        var sender = _serviceBusClient.CreateSender(@event.Queue);
        var json = JsonSerializer.Serialize(@event);

        try
        {
            await sender.SendMessageAsync(new ServiceBusMessage(json), cancellationToken);
        }
        catch
        {
            throw;
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }

    public async Task Publish<T>(T @event, DateTimeOffset deliversAt, CancellationToken cancellationToken = default) where T : Event
    {
        var sender = _serviceBusClient.CreateSender(@event.Queue);
        var json = JsonSerializer.Serialize(@event);

        try
        {
            await sender.ScheduleMessageAsync(new ServiceBusMessage(json), deliversAt, cancellationToken);
        }
        catch
        {
            throw;
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }
}

