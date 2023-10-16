using Glufs.Reviews.Domain.Products.Models;

namespace Glufs.Reviews.Domain.Products;

public interface IProductsRepository
{
    Task<Product> Get(long id, CancellationToken cancellationToken = default);
}

