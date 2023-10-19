using Glufs.Reviews.Jobs.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Glufs.Reviews.Jobs;

public static class Configuration
{
    public static void ConfigureJobs(this IServiceCollection services)
    {
        services.AddTriggersAndJobs();
    }
}