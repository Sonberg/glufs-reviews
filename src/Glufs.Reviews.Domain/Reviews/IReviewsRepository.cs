using Glufs.Reviews.Domain.Reviews.Models;

namespace Glufs.Reviews.Domain.Reviews;

public interface IReviewsRepository
{
    Task<ICollection<Review>> Get(ICollection<string> ids, CancellationToken cancellationToken = default);
}

