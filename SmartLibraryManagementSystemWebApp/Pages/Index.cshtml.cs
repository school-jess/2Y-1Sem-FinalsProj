// using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
// using System.Net.Http;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        using (var httpClient = new HttpClient())
        {
            var resp = await httpClient.GetAsync("http://localhost:5138/api");
            if (resp.IsSuccessStatusCode)
            {

            }
        }
    }
}
