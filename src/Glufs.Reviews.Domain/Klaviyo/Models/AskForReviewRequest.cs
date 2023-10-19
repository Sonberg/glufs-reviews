namespace Glufs.Reviews.Domain.Klaviyo.Models;

public record AskForReviewRequest
{
    public required string ReviewRequestId { get; set; }

    public required string OrderId { get; set; }

    public required string Email { get; set; }

    public string? Phone { get; set; }
}

