using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsMonitorModel : PageModel
{
    private readonly ILogger<InjectedIOptionsMonitorModel> _logger;
    public MarkdownConverter MarkdownConverterSettings;

    public InjectedIOptionsMonitorModel(ILogger<InjectedIOptionsMonitorModel> logger, IOptionsMonitor<MarkdownConverter> markdown)
    {
        _logger = logger;
        MarkdownConverterSettings = markdown.CurrentValue;
        
    }

    public void OnGet()
    {

    }
}
