using System.ComponentModel.DataAnnotations;

public class WebHookSettings
{
    [Required, Url]
    public string WebhookUrl { get; set; }
    [Required]
    public string DisplayName { get; set; }
    public bool Enabled { get; set; }
}