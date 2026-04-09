using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsModel : PageModel
{
    private readonly ILogger<InjectedIOptionsModel> _logger;
    public FileOptions PdfOptions = new FileOptions();
    public FileOptions DocOptions = new FileOptions();
    public FileOptions HtmlOptions = new FileOptions();

    public InjectedIOptionsModel(ILogger<InjectedIOptionsModel> logger,  IOptions<FileOptions> fileOptions)
    {
        _logger = logger;
        PdfOptions = fileOptions.Value;
        // DocOptions = fileOptions.Get("doc");
        // HtmlOptions = fileOptions.Get("html");
    }

    public void OnGet()
    {

    }
}
