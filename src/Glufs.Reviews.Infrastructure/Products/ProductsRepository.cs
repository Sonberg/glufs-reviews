using System.Text.Json;
using Glufs.Reviews.Domain.Products;
using Glufs.Reviews.Domain.Products.Models;
using Glufs.Reviews.Infrastructure.Policies;

namespace Glufs.Reviews.Infrastructure.Products;

public class ProductsRepository : IProductsRepository
{
    private record ProductResponse(Product Product);

    private readonly HttpClient _httpClient;

    public ProductsRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Product> Get(long id, CancellationToken cancellationToken = default)
    {
        var url = $"/admin/api/2023-07/products/{id}.json";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<ProductResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance

        });

        return data!.Product;
    }
}

