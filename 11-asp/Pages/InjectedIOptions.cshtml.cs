using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsModel : PageModel
{
    private readonly ILogger<InjectedIOptionsModel> _logger;
    public FileOptions PdfOptions;
    public FileOptions DocOptions;
    public FileOptions HTMLOptions;

    public InjectedIOptionsModel(ILogger<InjectedIOptionsModel> logger, IOptions<FileOptions> pdfOptions)
    {
        _logger = logger;
        PdfOptions = pdfOptions.Value;
        DocOptions = new FileOptions();
        HTMLOptions = new FileOptions();
    }

    public void OnGet()
    {

    }
}
