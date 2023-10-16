using Azure.Messaging.ServiceBus;
using Glufs.Reviews.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Glufs.Reviews.Jobs;


public interface IJob<T>
{
    Task RunAsync(T data, CancellationToken cancellationToken = default);
}

public abstract class Trigger<TEvent> : IHostedService where TEvent : Event
{
    private readonly ServiceBusClient _client;

    private readonly ServiceBusProcessor _processor;

    private readonly IServiceProvider _serviceProvider;

    public Trigger(IServiceProvider serviceProvider, ServiceBusClient client)
    {
        _serviceProvider = serviceProvider;
        _client = client;

        _processor = _client.CreateProcessor(Queue, new ServiceBusProcessorOptions());

    }

    public abstract string Queue { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var log = scope.ServiceProvider.GetService<ILogger<Trigger<TEvent>>>();
            var data = args.Message.Body.ToObjectFromJson<TEvent>();

            log?.LogInformation("Queue: {Queue}. Payload: {data}", Queue, data);

            await scope.ServiceProvider
                .GetRequiredService<IJob<TEvent>>()
                .RunAsync(args.Message.Body.ToObjectFromJson<TEvent>(), args.CancellationToken);
        }
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());

        return Task.CompletedTask;
    }
}


