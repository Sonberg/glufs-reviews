using System.Net.Http.Headers;
using Glufs.Reviews.Domain.Klaviyo;
using Glufs.Reviews.Domain.Orders;
using Glufs.Reviews.Domain.Products;
using Glufs.Reviews.Domain.ReviewRequests;
using Glufs.Reviews.Domain.Reviews;
using Glufs.Reviews.Infrastructure.Klaviyo;
using Glufs.Reviews.Infrastructure.Options;
using Glufs.Reviews.Infrastructure.Orders;
using Glufs.Reviews.Infrastructure.Products;
using Glufs.Reviews.Infrastructure.ReviewRequests;
using Glufs.Reviews.Infrastructure.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Glufs.Reviews.Infrastructure;

public static class Configuration
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<KlaviyoOptions>()
            .Bind(configuration.GetSection(KlaviyoOptions.SectionName))
            .ValidateDataAnnotations();

        services
           .AddOptions<ShopifyAdminOptions>()
           .Bind(configuration.GetSection(ShopifyAdminOptions.SectionName))
           .ValidateDataAnnotations();

        services.AddHttpClient<IKlaviyo, KlaviyoClient>((sp, opt) =>
        {
            var options = sp.GetRequiredService<IOptions<KlaviyoOptions>>();
            var apiKey = options.Value.ApiKey;

            opt.BaseAddress = new Uri(" https://a.klaviyo.com");
            opt.DefaultRequestHeaders.Add("Authorization", $"Klaviyo-API-Key {apiKey}");
            opt.DefaultRequestHeaders.Add("revision", "2023-10-15");
        });

        services.AddHttpClient<IOrdersRepository, OrdersRepository, ShopifyAdminOptions>((client, opt) =>
        {
            client.BaseAddress = opt.Domain!;
            client.DefaultRequestHeaders.Add("X-Shopify-Access-Token", opt.AccessToken);
        });

        services.AddHttpClient<IProductsRepository, ProductsRepository, ShopifyAdminOptions>((client, opt) =>
        {
            client.BaseAddress = opt.Domain!;
            client.DefaultRequestHeaders.Add("X-Shopify-Access-Token", opt.AccessToken);
        });

        services.AddScoped<IReviewRequestsRepository, ReviewRequestsRepository>();
        services.AddScoped<IReviewsRepository, ReviewsRepository>();
    }

    private static void AddHttpClient<TClient, TImplementation, TOptions>(this IServiceCollection services,
       Action<HttpClient, TOptions> configure) where TClient : class
       where TImplementation : class, TClient
       where TOptions : class
    {

        services
            .AddHttpClient<TClient, TImplementation>((sp, c) =>
            {
                configure(c, sp.GetRequiredService<IOptions<TOptions>>().Value);

                c.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            });
    }
}