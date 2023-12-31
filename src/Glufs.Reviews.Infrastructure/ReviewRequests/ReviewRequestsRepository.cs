﻿using Glufs.Reviews.Domain.ReviewRequests;
using Glufs.Reviews.Domain.ReviewRequests.Models;
using Glufs.Reviews.Infrastructure.Factories;

namespace Glufs.Reviews.Infrastructure.ReviewRequests;

public class ReviewRequestsRepository : IReviewRequestsRepository
{
    private readonly ISupabaseClientFactory _factory;

    public ReviewRequestsRepository(ISupabaseClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<ReviewRequest?> Create(ReviewRequest request, CancellationToken cancellationToken)
    {
        var client = _factory.GetClient();
        var response = await client.From<ReviewRequest>().Insert(request, null, cancellationToken);

        return response.Model;
    }

    public  async Task<ICollection<ReviewRequest>> Get(CancellationToken cancellationToken)
    {
        var client = _factory.GetClient();
        var response = await client
            .From<ReviewRequest>()
            .Filter(x => x.OrderId!, Postgrest.Constants.Operator.NotEqual, null)
            .Order(x => x.CreatedAt!, Postgrest.Constants.Ordering.Descending)
            .Get(cancellationToken);

        return response.Models;
    }

    public async Task<ICollection<ReviewRequest>> GetByCustomerId(string customerId, CancellationToken cancellationToken)
    {
        var client = _factory.GetClient();
        var response = await client
            .From<ReviewRequest>()
            .Filter(x => x.CustomerId!, Postgrest.Constants.Operator.Equals, customerId)
            .Get(cancellationToken);

        return response.Models;
    }

    public async Task<bool> IsReviewRequested(string orderId, CancellationToken cancellationToken)
    {
        var client = _factory.GetClient();
        var count = await client
            .From<ReviewRequest>()
            .Filter(x => x.OrderId!, Postgrest.Constants.Operator.Equals, orderId)
            .Count(Postgrest.Constants.CountType.Exact,  cancellationToken);

        return count != 0;
    }
}

