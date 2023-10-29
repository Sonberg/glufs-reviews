using Glufs.Reviews.Domain.Orders.Models;

namespace Glufs.Reviews.Domain.Orders;

public interface IOrdersRepository
{
    Task<Order> Get(long id, CancellationToken cancellationToken = default);

    Task<ICollection<Order>> Get(CancellationToken cancellationToken = default);

    Task SetTags(long id, List<string> tags, CancellationToken cancellationToken = default);
}

