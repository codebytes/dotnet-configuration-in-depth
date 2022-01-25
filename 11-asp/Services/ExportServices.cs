using Microsoft.Extensions.Options;
namespace SampleAsp.Services;

public interface IExportService
{
    MarkdownConverter GetMarkdownConverter();
}

public class ExportService : IExportService
{
    private readonly MarkdownConverter _markdown;
    public ExportService(IOptionsMonitor<MarkdownConverter> markdown)
    {
        _markdown = markdown.CurrentValue;
    }

    public MarkdownConverter GetMarkdownConverter()
    {
        return _markdown;
    }
}