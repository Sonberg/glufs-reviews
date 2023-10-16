using Glufs.Reviews.Domain.Orders.Models;

namespace Glufs.Reviews.Domain.Orders;

public interface IOrdersRepository
{
    Task<Order> Get(long id, CancellationToken cancellationToken = default);
}

