using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsSnapshotModel : PageModel
{
    private readonly ILogger<InjectedIOptionsSnapshotModel> _logger;
    public FileOptions PdfOptions;
    public InjectedIOptionsSnapshotModel(ILogger<InjectedIOptionsSnapshotModel> logger, IOptionsSnapshot<FileOptions> pdfOptions)
    {
        _logger = logger;
        PdfOptions = pdfOptions.Value;
    }

    public void OnGet()
    {

    }
}
