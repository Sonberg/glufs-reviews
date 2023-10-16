using System.Text.Json;
using Glufs.Reviews.Domain.Orders;
using Glufs.Reviews.Domain.Orders.Models;
using Glufs.Reviews.Infrastructure.Policies;

namespace Glufs.Reviews.Infrastructure.Orders;

public class OrdersRepository : IOrdersRepository
{
    private readonly HttpClient _httpClient;

	public OrdersRepository(HttpClient httpClient)
	{
        _httpClient = httpClient;
	}

    public async Task<Order> Get(long id, CancellationToken cancellationToken = default)
    {
        var url = $"/admin/api/2023-07/orders/{id}.json";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance

        });

        throw new NotImplementedException();
    }
}

