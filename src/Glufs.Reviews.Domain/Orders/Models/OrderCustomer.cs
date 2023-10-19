namespace Glufs.Reviews.Domain.Orders.Models;

public class OrderCustomer
{
	public long Id { get; set; }

	public required string AdminGraphqlApiId { get; set; }

	public required string FirstName { get; set; }

	public required string LastName { get; set; }

	public required string Email { get; set; }

	public string? Phone { get; set; }
}

