using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleAsp.Pages;

public class InjectConfigInRazorModel : PageModel
{
    private readonly ILogger<InjectConfigInRazorModel> _logger;
    public InjectConfigInRazorModel(ILogger<InjectConfigInRazorModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
