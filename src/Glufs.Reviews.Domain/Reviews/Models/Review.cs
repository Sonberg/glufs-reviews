using Postgrest.Attributes;
using Postgrest.Models;

namespace Glufs.Reviews.Domain.Reviews.Models;

[Table("reviews")]
public class Review : BaseModel
{
    public Review()
    {
        Products = Array.Empty<string>();
    }

    [PrimaryKey("id")]
    public string? Id { get; set; }

    [Column("order_id")]
    public string? OrderId { get; set; }

    [Column("customer_name")]
    public string? CustomerName { get; set; }

    [Column("products")]
    public string[] Products { get; set; }
}
