using System.ComponentModel.DataAnnotations;

namespace Glufs.Reviews.Infrastructure.Options;

public class ShopifyAdminOptions
{
    public static string SectionName = "Shopify:Admin";

    [Required]
    public Uri? Domain { get; set; }

    [Required]
    public string? AccessToken { get; set; }
}

