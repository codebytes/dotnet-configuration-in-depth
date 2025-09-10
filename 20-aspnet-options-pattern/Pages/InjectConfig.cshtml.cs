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
        var pdf = configuration.GetSection("MarkdownConverter:pdf").Get<FileOptions>();
        var doc = configuration.GetSection("MarkdownConverter:doc").Get<FileOptions>();
        var html = configuration.GetSection("MarkdownConverter:html").Get<FileOptions>();
        if(pdf is not null)
        {
            Options.Add(pdf);
        }
        if(doc is not null)
        {
            Options.Add(doc);
        }
        if(html is not null)
        {
            Options.Add(html);
        }
    }

    public void OnGet()
    {

    }
}
