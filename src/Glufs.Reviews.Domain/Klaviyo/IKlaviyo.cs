namespace Glufs.Reviews.Domain.Klaviyo;

public interface IKlaviyo
{
    Task AskForReview(CancellationToken cancellationToken = default);
}

