using Glufs.Reviews.Domain.Reviews;
using Glufs.Reviews.Domain.Reviews.Models;

namespace Glufs.Reviews.Infrastructure.Reviews;

public class ReviewsRepository : IReviewsRepository
{
    public ReviewsRepository()
    {
    }

    public Task<ICollection<Review>> GetByCustomer(string customerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

