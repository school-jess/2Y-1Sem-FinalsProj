using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
// using System.Net.Http;
using System.Text.Json;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class IndexModel : PageModel
{
    public List<BookUpdateDto> Books { get; set; }
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
                Books = JsonSerializer.Deserialize<List<BookUpdateDto>>(respContent);
            }
        }
    }
}
