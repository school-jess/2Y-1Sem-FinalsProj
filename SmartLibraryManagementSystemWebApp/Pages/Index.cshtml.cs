using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class IndexModel : PageModel
{
    public bool IsAdmin { get; set; }
    public List<BookUpdateDto> Books { get; set; }
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var getBooks = await httpClient.GetAsync("http://localhost:5138/api/Book");
            if (getBooks.IsSuccessStatusCode)
            {
                var getBooksContent = await getBooks.Content.ReadAsStringAsync();
                Books = JsonSerializer.Deserialize<List<BookUpdateDto>>(getBooksContent, options);
            }

            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                var getUser =
                    await httpClient.GetAsync(
                        $"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}");
                if (!getUser.IsSuccessStatusCode) return;
                var getUserContext = await getUser.Content.ReadAsStringAsync();
                UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserContext, options);
                IsAdmin = user.IsAdmin;
            }
        }
    }
}
