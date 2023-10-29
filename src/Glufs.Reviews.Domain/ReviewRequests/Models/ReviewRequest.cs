using Postgrest.Attributes;
using Postgrest.Models;

namespace Glufs.Reviews.Domain.ReviewRequests.Models;

[Table("review_requests")]
public class ReviewRequest : BaseModel
{
    [PrimaryKey("id")]
    public string? Id { get; set; }

    [Column("review_id")]
    public string? ReviewId { get; set; }

    [Column("order_id")]
    public string? OrderId { get; set; }

    [Column("customer_name")]
    public string? CustomerName { get; set; }

    [Column("customer_id")]
    public string? CustomerId { get; set; }

    [Column("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }
}

