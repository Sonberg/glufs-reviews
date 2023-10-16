namespace Glufs.Reviews.Domain.Events;

public record SendReviewEmailEvent : Event
{
    public override string Queue => "send-review-email";

    public required long OrderId { get; set; }
}