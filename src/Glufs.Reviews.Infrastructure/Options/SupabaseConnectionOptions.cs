using System.ComponentModel.DataAnnotations;

namespace Glufs.Reviews.Infrastructure.Options;

public class SupabaseConnectionOptions
{
    public static string SectionName = "Supabase";

    [Required]
    public string? Url { get; set; }

    [Required]
    public string? Key { get; set; }
}
