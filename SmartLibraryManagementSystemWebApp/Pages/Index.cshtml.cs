using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class IndexModel : PageModel
{
    public bool IsAdmin { get; set; } = false;
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
            var getBooks = await httpClient.GetAsync("http://localhost:5138/api/Book");
            if (getBooks.IsSuccessStatusCode)
            {
                var getBooksContent = await getBooks.Content.ReadAsStringAsync();
                Books = JsonSerializer.Deserialize<List<BookUpdateDto>>(getBooksContent);
            }
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{HttpContext.Session.GetString("LogInName")}");
                if (!getUser.IsSuccessStatusCode) return; // todo
                var getUserContext = await getUser.Content.ReadAsStringAsync();
                UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserContext);
                IsAdmin = user.IsAdmin;
            }
        }
    }
}
