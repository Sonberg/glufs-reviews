using System.ComponentModel.DataAnnotations;

namespace Glufs.Reviews.Jobs.Options;

public class ServiceBusOptions
{
    public static string SectionName = "ServiceBus";

    [Required]
    public string? ConnectionString { get; init; }
}

