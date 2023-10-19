using Glufs.Reviews.Domain.Klaviyo.Models;

namespace Glufs.Reviews.Domain.Klaviyo;

public interface IKlaviyo
{
    Task AskForReview(AskForReviewRequest request, CancellationToken cancellationToken = default);
}

