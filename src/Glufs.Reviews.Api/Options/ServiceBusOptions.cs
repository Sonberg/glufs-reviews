using System.ComponentModel.DataAnnotations;

namespace Glufs.Reviews.Api.Options;

public class ServiceBusOptions
{
    public static string SectionName = "ServiceBus";

    [Required]
    public string? ConnectionString { get; init; }
}

