using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleAsp.Pages;

public class InjectConfigModel : PageModel
{
    private readonly ILogger<InjectConfigModel> _logger;
    public List<FileOptions> Options {get;private set;} = new List<FileOptions>();
    public InjectConfigModel(ILogger<InjectConfigModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        Options.Add(configuration.GetSection("MarkdownConverter:pdf").Get<FileOptions>());
        Options.Add(configuration.GetSection("MarkdownConverter:doc").Get<FileOptions>());
        Options.Add(configuration.GetSection("MarkdownConverter:html").Get<FileOptions>());
    }

    public void OnGet()
    {

    }
}
