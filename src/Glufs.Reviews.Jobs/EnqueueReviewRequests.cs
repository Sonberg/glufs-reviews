using EasyCronJob.Abstractions;
using Glufs.Reviews.Domain.Events;
using Glufs.Reviews.Domain.Orders;
using Glufs.Reviews.Domain.Orders.Models;
using Glufs.Reviews.Domain.ReviewRequests;
using Microsoft.Extensions.DependencyInjection;

namespace Glufs.Reviews.Jobs;

public class EnqueueReviewRequests : CronJobService
{
    private readonly IServiceProvider _serviceProvider;

    private const string ReviewRequestedTag = "ReviewRequested";

    private const string NoReviewTag = "NoReview";

    public EnqueueReviewRequests(
        ICronConfiguration<EnqueueReviewRequests> cronConfiguration,
        IServiceProvider serviceProvider) : base(cronConfiguration.CronExpression, cronConfiguration.TimeZoneInfo, cronConfiguration.CronFormat)
    {
        _serviceProvider = serviceProvider;
    }


    public async override Task DoWork(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var ordersRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
        var reviewRequestsRepository = scope.ServiceProvider.GetRequiredService<IReviewRequestsRepository>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

        var orders = await ordersRepository.Get(cancellationToken);
        var reviewRequests = await reviewRequestsRepository.Get(cancellationToken);

        var numberOfDays = 10;
        var needsReview = new List<Order>();
        var now = DateTimeOffset.Now;

        foreach (var order in orders)
        {
            var tagsWithReviewRequested = order.Tags.Append(ReviewRequestedTag).ToList();
            var tagsWithNoReview = order.Tags.Append(NoReviewTag).ToList();

            if (order.ClosedAt is null)
            {
                continue;
            }

            if (order.ClosedAt.Value.AddDays(numberOfDays) > now)
            {
                continue;
            }

            if (order.Tags.Contains(ReviewRequestedTag))
            {
                continue;
            }

            if (order.Tags.Contains(NoReviewTag))
            {
                continue;
            }

            if (order.FulfillmentStatus != "fulfilled")
            {
                continue;
            }

            if (order.FinancialStatus != "paid")
            {
                await ordersRepository.SetTags(order.Id, tagsWithNoReview, cancellationToken);
                continue;
            }

            if (order.Customer.Email is null)
            {
                await ordersRepository.SetTags(order.Id, tagsWithNoReview, cancellationToken);

                continue;
            }

            if (reviewRequests.Any(x => x.OrderId == order.AdminGraphqlApiId))
            {
                await ordersRepository.SetTags(order.Id, tagsWithNoReview, cancellationToken);

                continue;
            }

            needsReview.Add(order);
        }

        foreach (var order in needsReview)
        {
            await eventPublisher.Publish(new SendReviewEmailEvent { OrderId = order.Id }, cancellationToken);
        }
    }
}

