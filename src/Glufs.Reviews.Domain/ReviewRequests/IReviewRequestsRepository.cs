using Glufs.Reviews.Domain.ReviewRequests.Models;

namespace Glufs.Reviews.Domain.ReviewRequests;

public interface IReviewRequestsRepository
{
    Task<ICollection<ReviewRequest>> GetByCustomerId(string customerId, CancellationToken cancellationToken);

    Task<ReviewRequest?> Create(ReviewRequest request, CancellationToken cancellationToken);
}

