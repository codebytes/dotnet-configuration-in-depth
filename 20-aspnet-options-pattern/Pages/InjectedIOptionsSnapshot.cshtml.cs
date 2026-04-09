using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsSnapshotModel : PageModel
{
    private readonly ILogger<InjectedIOptionsSnapshotModel> _logger;
    public MarkdownConverter MarkdownConverterSettings;
    public InjectedIOptionsSnapshotModel(ILogger<InjectedIOptionsSnapshotModel> logger, IOptionsSnapshot<MarkdownConverter> markdown)
    {
        _logger = logger;
        MarkdownConverterSettings = markdown.Value;
    }

    public void OnGet()
    {

    }
}
