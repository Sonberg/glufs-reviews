using Glufs.Reviews.Domain.Events;
using Glufs.Reviews.Domain.Klaviyo;
using Glufs.Reviews.Domain.Klaviyo.Models;
using Glufs.Reviews.Domain.Orders;
using Glufs.Reviews.Domain.Products;
using Glufs.Reviews.Domain.ReviewRequests;
using Glufs.Reviews.Domain.ReviewRequests.Models;
using Glufs.Reviews.Domain.Reviews;

namespace Glufs.Reviews.Jobs.SendReviewEmail;

public class SendReviewEmailJob : IJob<SendReviewEmailEvent>
{

    private readonly IOrdersRepository _ordersRepository;

    private readonly IReviewRequestsRepository _reviewRequestsRepository;

    private readonly IReviewsRepository _reviewsRepository;

    private readonly IProductsRepository _productsRepository;

    private readonly IKlaviyo _klaviyo;

    public SendReviewEmailJob(
        IOrdersRepository ordersRepository,
        IReviewRequestsRepository reviewRequestsRepository,
        IReviewsRepository reviewsRepository,
        IProductsRepository productsRepository,
        IKlaviyo klaviyo)
    {
        _ordersRepository = ordersRepository;
        _reviewRequestsRepository = reviewRequestsRepository;
        _reviewsRepository = reviewsRepository;
        _productsRepository = productsRepository;
        _klaviyo = klaviyo;
    }

    public async Task RunAsync(SendReviewEmailEvent data, CancellationToken cancellationToken = default)
    {
        var order = await _ordersRepository.Get(data.OrderId, cancellationToken);
        var orderLines = order.LineItems.Where(x => x.IsFulfilled).ToList();
        var productsTask = orderLines.Select(x => _productsRepository.Get(x.ProductId, cancellationToken));
        var products = await Task.WhenAll(productsTask);
        var handles = products.Select(x => x.Handle).Order().ToArray();

        var reviews = await _reviewsRepository.GetByCustomer(order.Customer.AdminGraphqlApiId, cancellationToken);
        var reviewedProducts = reviews.SelectMany(x => x.Products).Order().ToArray();

        if (handles.SequenceEqual(reviewedProducts))
        {
            return;
        }

        var request = await _reviewRequestsRepository.Create(new ReviewRequest
        {
            CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}",
            OrderId = order.AdminGraphqlApiId
        }, cancellationToken);

        await _klaviyo.AskForReview(new AskForReviewRequest
        {
            ReviewRequestId = request!.Id!,
            OrderId = order.AdminGraphqlApiId,
            Email = order.Customer.Email,
            Phone = order.Customer.Phone

        }, cancellationToken);
    }
}

