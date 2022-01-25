using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleAsp.Pages;

public class ProvidersModel : PageModel
{
    private readonly ILogger<ProvidersModel> _logger;
    public IEnumerable<string> Providers {get; private set;}
    public ProvidersModel(ILogger<ProvidersModel> logger, IConfiguration configRoot)
    {
        _logger = logger;
        Providers = ((IConfigurationRoot)configRoot).Providers.Select(p => p.ToString());
    }

    public void OnGet()
    {

    }
}
