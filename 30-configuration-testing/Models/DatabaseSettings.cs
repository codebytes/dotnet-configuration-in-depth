using System.ComponentModel.DataAnnotations;

namespace ConfigurationTesting.Models;

public class DatabaseSettings
{
    public const string SectionName = "Database";

    [Required]
    [MinLength(10)]
    public string ConnectionString { get; set; } = string.Empty;

    [Range(1, 3600)]
    public int TimeoutSeconds { get; set; } = 30;

    public bool EnableRetries { get; set; } = true;

    [Range(1, 10)]
    public int MaxRetries { get; set; } = 3;
}