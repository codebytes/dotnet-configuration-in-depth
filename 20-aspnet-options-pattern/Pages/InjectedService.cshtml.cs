using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleAsp.Services;

namespace SampleAsp.Pages;

public class InjectedServiceModel : PageModel
{
    private readonly ILogger<InjectedServiceModel> _logger;
    public MarkdownConverter MarkdownConverterSettings;

    public InjectedServiceModel(ILogger<InjectedServiceModel> logger, IExportService exportService)
    {
        _logger = logger;
        MarkdownConverterSettings = exportService.GetMarkdownConverter();
    }

    public void OnGet()
    {

    }
}
