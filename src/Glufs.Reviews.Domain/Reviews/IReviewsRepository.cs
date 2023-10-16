using Glufs.Reviews.Domain.Reviews.Models;

namespace Glufs.Reviews.Domain.Reviews;

public interface IReviewsRepository
{
    Task<ICollection<Review>> GetByCustomer(string customerId, CancellationToken cancellationToken = default);
}

