using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class IndexModel : PageModel
{
    public bool IsAdmin { get; set; }
    public List<BookUpdateDto> Books { get; set; }
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel()
    {
        public int BookId { get; set; }
    }

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("IsAdmin") != "true") return NotFound();
        if (!ModelState.IsValid) throw new InvalidOperationException("invalid input");
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var deleteBook = await httpClient.DeleteAsync($"http://localhost:5138/api/Book/{Input.BookId}");
            if (deleteBook.IsSuccessStatusCode) throw new InvalidOperationException("couldn't delete book");
        }
        return Page();
    }
}
