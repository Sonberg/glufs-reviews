using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Glufs.Reviews.Jobs.Extensions;
using Glufs.Reviews.Jobs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Glufs.Reviews.Jobs;

public static class Configuration
{
    public static void ConfigureJobs(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        services
          .AddOptions<ServiceBusOptions>()
          .Bind(configuration.GetSection(ServiceBusOptions.SectionName))
          .ValidateDataAnnotations();

        services.AddSingleton(sp =>
        {
            if (isDevelopment)
            {
                return new ServiceBusClient("glufs-dev-sbus.servicebus.windows.net", new DefaultAzureCredential());
            }
            else
            {
                return new ServiceBusClient(sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value.ConnectionString!);
            }

        });

        services.AddTriggersAndJobs();

    }
}