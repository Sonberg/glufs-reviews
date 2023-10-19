using Glufs.Reviews.Domain.Klaviyo;
using Glufs.Reviews.Domain.Klaviyo.Models;
using Microsoft.Extensions.Options;

namespace Glufs.Reviews.Infrastructure.Klaviyo;

public class KlaviyoClient : KlaviyoClientBase, IKlaviyo
{
    private readonly KlaviyoOptions _options;

    public KlaviyoClient(HttpClient httpClient, IOptions<KlaviyoOptions> options) : base(httpClient)
    {
        _options = options.Value;
    }

    public async Task AskForReview(AskForReviewRequest request, CancellationToken cancellationToken = default)
    {
        var profile = new Profile(request.Email, request.Phone);
        var profileId = await UpsertProfile(profile, new
        {
            reviewRequestId = request.ReviewRequestId
        }, cancellationToken);

        await AddToList(_options.ReviewCustomersListId, profileId, cancellationToken);
        await TrackEvent(profileId, "Review request", new
        {
            reviewRequestId = request.ReviewRequestId,
            orderId = request.OrderId,
        }, cancellationToken);
    }
}