public class MarkdownConverter
{
    public string MarkdownStyle { get; set; } = string.Empty;
    public FileOptions Pdf { get; set; } = new FileOptions();
    public FileOptions Doc { get; set; } = new FileOptions();
    public FileOptions Html { get; set; } = new FileOptions();
}
