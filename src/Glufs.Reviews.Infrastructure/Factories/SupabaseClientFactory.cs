using Glufs.Reviews.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Supabase;

namespace Glufs.Reviews.Infrastructure.Factories;

public interface ISupabaseClientFactory
{
    Client GetClient();

}

public class SupabaseClientFactory : ISupabaseClientFactory
{
    private readonly SupabaseConnectionOptions _options;

    public SupabaseClientFactory(IOptions<SupabaseConnectionOptions> options)
    {
        _options = options.Value;
    }

    public Client GetClient()
    {
        return new Client(_options.Url!, _options.Key);
    }

    public SupabaseConnectionOptions Options => _options;
}
