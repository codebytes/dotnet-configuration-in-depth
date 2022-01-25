using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsMonitorModel : PageModel
{
    private readonly ILogger<InjectedIOptionsMonitorModel> _logger;
    public FileOptions PdfOptions;
    public FileOptions DocOptions;
    public FileOptions HtmlOptions;
    
    public InjectedIOptionsMonitorModel(ILogger<InjectedIOptionsMonitorModel> logger,  IOptionsSnapshot<FileOptions> fileOptions)
    {
        _logger = logger;
        PdfOptions = fileOptions.Get("pdf");
        DocOptions = fileOptions.Get("doc");
        HtmlOptions = fileOptions.Get("html");
    }

    public void OnGet()
    {

    }
}
