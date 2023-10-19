using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Glufs.Reviews.Jobs.Extensions;


public static class ServiceCollectionExtensions
{
    public static void AddTriggersAndJobs(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(ServiceCollectionExtensions));
        var triggers = assembly!.DefinedTypes
            .Where(x => x.BaseType?.IsGenericType is true)
            .Where(x => x.BaseType?.GetGenericTypeDefinition() == typeof(Trigger<>))
            .ToList();

        foreach (var trigger in triggers)
        {
            services.AddTransient(typeof(IHostedService), trigger);
        }

        var jobs = assembly!.DefinedTypes
            .Where(x => x.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IJob<>)))
            .ToList();

        foreach (var job in jobs)
        {
            services.AddTransient(job.ImplementedInterfaces.Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IJob<>)), job);
        }
    }
}
