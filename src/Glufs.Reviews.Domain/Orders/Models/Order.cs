namespace Glufs.Reviews.Domain.Orders.Models;

public class Order
{
	public required string AdminGraphqlApiId { get; set; }

    public required OrderCustomer Customer { get; set; }

	public required List<OrderLineItem> LineItems { get; set; }
}

