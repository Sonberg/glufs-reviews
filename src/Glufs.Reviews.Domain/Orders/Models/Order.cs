using System.Text.Json;
using System.Text.Json.Serialization;

namespace Glufs.Reviews.Domain.Orders.Models;

public class TagsJsonConverter : JsonConverter<List<string>>
{
    public override List<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (value is null)
        {
            return new();
        }

        return value.Split(", ").ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        if (value.Any())
        {
            writer.WriteStringValue(string.Join(", ", value));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

public class Order
{
    public Order()
    {
        Tags = new();
    }

    public long Id { get; set; }

    public required string AdminGraphqlApiId { get; set; }

    public required OrderCustomer Customer { get; set; }

    public required List<OrderLineItem> LineItems { get; set; }

    [JsonConverter(typeof(TagsJsonConverter))]
    public List<string> Tags { get; set; }

    public string? SourceName { get; set; }

    public string? FinancialStatus { get; set; }

    public string? FulfillmentStatus { get; set; }

    public DateTimeOffset? ClosedAt { get; set; }
}

