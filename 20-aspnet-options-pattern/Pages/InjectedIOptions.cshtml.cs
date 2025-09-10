using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace SampleAsp.Pages;

public class InjectedIOptionsModel : PageModel
{
    private readonly ILogger<InjectedIOptionsModel> _logger;
    public MarkdownConverter MarkdownConverterSettings;

    public InjectedIOptionsModel(ILogger<InjectedIOptionsModel> logger, IOptions<MarkdownConverter> markdown)
    {
        _logger = logger;
        MarkdownConverterSettings = markdown.Value;
    }

    public void OnGet()
    {

    }
}
