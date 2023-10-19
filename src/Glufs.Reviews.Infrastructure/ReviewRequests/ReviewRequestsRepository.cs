using Glufs.Reviews.Domain.ReviewRequests;
using Glufs.Reviews.Domain.ReviewRequests.Models;

namespace Glufs.Reviews.Infrastructure.ReviewRequests;

public class ReviewRequestsRepository : IReviewRequestsRepository
{
    public ReviewRequestsRepository()
    {
    }

    public Task<ReviewRequest?> Create(ReviewRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<ReviewRequest>> GetByCustomerId(string customerId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

