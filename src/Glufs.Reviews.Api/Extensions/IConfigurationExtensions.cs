using Azure.Identity;

namespace Glufs.Reviews.Api.Extensions;

public static class IConfigurationExtensions
{
    public static bool TryGetValue<T>(this IConfiguration configuration, string key, out T? value)
    {
        value = configuration.GetValue<T?>(key);

        return value != null;
    }

    public static DefaultAzureCredential GetAzureCredentials(this IConfiguration configuration)
    {
        return new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = configuration.GetValue<string>("ManagedIdentityClientId"),
            TenantId = configuration.GetValue<string>("TenantId")
        });
    }
}
