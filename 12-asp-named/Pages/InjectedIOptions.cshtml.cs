using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsModel : PageModel
{
    private readonly ILogger<InjectedIOptionsModel> _logger;
    public FileOptions PdfOptions;
    public FileOptions DocOptions;
    public FileOptions HtmlOptions;

    public InjectedIOptionsModel(ILogger<InjectedIOptionsModel> logger,  IOptions<FileOptions> fileOptions)
    {
        _logger = logger;
        PdfOptions = fileOptions.Value;
        DocOptions = new FileOptions();
        HtmlOptions = new FileOptions();
    }

    public void OnGet()
    {

    }
}
