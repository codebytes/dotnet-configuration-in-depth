using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsMonitorModel : PageModel
{
    private readonly ILogger<InjectedIOptionsMonitorModel> _logger;
    public FileOptions PdfOptions;
    
    public InjectedIOptionsMonitorModel(ILogger<InjectedIOptionsMonitorModel> logger, IOptionsMonitor<FileOptions> pdfOptions)
    {
        _logger = logger;
        PdfOptions = pdfOptions.CurrentValue;
    }

    public void OnGet()
    {

    }
}
