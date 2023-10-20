using Glufs.Reviews.Domain.Reviews;
using Glufs.Reviews.Domain.Reviews.Models;
using Glufs.Reviews.Infrastructure.Factories;

namespace Glufs.Reviews.Infrastructure.Reviews;

public class ReviewsRepository : IReviewsRepository
{
    private readonly ISupabaseClientFactory _factory;

    public ReviewsRepository(ISupabaseClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<ICollection<Review>> Get(ICollection<string> ids, CancellationToken cancellationToken = default)
    {
        var client = _factory.GetClient();
        var response = await client
            .From<Review>()
            .Filter(x => x.Id!, Postgrest.Constants.Operator.In, ids.Cast<object>().ToList())
            .Get(cancellationToken);

        return response.Models;
    }
}

