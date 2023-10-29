using System.Net.Http.Json;
using System.Text.Json;
using Glufs.Reviews.Domain.Orders;
using Glufs.Reviews.Domain.Orders.Models;
using Glufs.Reviews.Infrastructure.Policies;

namespace Glufs.Reviews.Infrastructure.Orders;

public class OrdersRepository : IOrdersRepository
{
    private record OrderResponse(Order Order);
    private record OrdersResponse(List<Order> Orders);

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
        var data = JsonSerializer.Deserialize<OrderResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance

        });

        return data!.Order;
    }

    public async Task<ICollection<Order>> Get(CancellationToken cancellationToken = default)
    {
        var url = $"/admin/api/2023-07/orders.json?status=closed&limit=250";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<OrdersResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance

        });

        return data!.Orders;
    }

    public async Task SetTags(long id, List<string> tags, CancellationToken cancellationToken = default)
    {
        var url = $"/admin/api/2023-07/orders/{id}.json";
        var content = JsonContent.Create(new
        {
            Order = new
            {
                Id = id,
                Tags = string.Join(", ", tags)
            }
        });

        var response = await _httpClient.PutAsync(url, content, cancellationToken);

        response.EnsureSuccessStatusCode();
       
    }
}

