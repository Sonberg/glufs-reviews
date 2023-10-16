namespace Glufs.Reviews.Domain.Orders.Models;

public class OrderLineItem
{
	public string? FulfillmentStatus { get; set; }

	public bool IsFulfilled => FulfillmentStatus == "fulfilled";

	public long ProductId { get; set; }
}

