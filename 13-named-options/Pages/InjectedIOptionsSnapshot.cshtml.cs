using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsSnapshotModel : PageModel
{
    private readonly ILogger<InjectedIOptionsSnapshotModel> _logger;
    public FileOptions PdfOptions;
    public FileOptions DocOptions;
    public FileOptions HtmlOptions;
    public InjectedIOptionsSnapshotModel(ILogger<InjectedIOptionsSnapshotModel> logger, IOptionsSnapshot<FileOptions> fileOptions)
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
