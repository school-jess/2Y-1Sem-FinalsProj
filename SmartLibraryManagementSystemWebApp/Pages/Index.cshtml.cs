using SmartLibraryManagementSystemClassLibrary.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class IndexModel : PageModel
{
    public List<Book> Books { get; set; }
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        using (var httpClient = new HttpClient())
        {
            var resp = await httpClient.GetAsync("http://localhost:5138/api/Book");
            if (resp.IsSuccessStatusCode)
            {
                var respContent = await resp.Content.ReadAsStringAsync();
            }
        }
    }
}
