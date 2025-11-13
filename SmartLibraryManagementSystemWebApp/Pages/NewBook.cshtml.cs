using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class NewBookModel : PageModel
{
    [BindProperty] public InputModel Input { get; set; }

    public class InputModel
    {
        [StringLength(30)] public string Name { get; set; }
        [StringLength(50)] public string Author { get; set; }
        [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }
        public string Synopsis { get; set; }
        [StringLength(10)] public string ClassificationId { get; set; }
        public int Copies { get; set; }
        [StringLength(10)] public string Genre { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        string userId = HttpContext.Session.GetString("UserId");
        using (var httpClient = new HttpClient())
        {
            var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{userId}");
            if (!getUser.IsSuccessStatusCode) return new StatusCodeResult(500);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getUserCont = await getUser.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserGet1Dto>(getUserCont, options);
            if (!user.IsAdmin) return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        using (var httpClient = new HttpClient())
        {
            BookCreationDto book = new BookCreationDto(Input.Name, Input.Author, Input.ReleaseDate, Input.Synopsis);
            string bookSerialized = JsonSerializer.Serialize(book);
            var bookHttpCont = new StringContent(bookSerialized, Encoding.UTF8, "application/json");
            var newBook = await httpClient.PostAsync("http://localhost:5138/api/Book", bookHttpCont);
            if (!newBook.IsSuccessStatusCode) throw new InvalidOperationException("error when creating new book");
            var getBookContent = await newBook.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var insertedBook = JsonSerializer.Deserialize<BookUpdateDto>(getBookContent, options);
            CatalogCreationDto catalog = new CatalogCreationDto(insertedBook.BookId, Input.Copies, Input.Genre,
                Input.ClassificationId, 0);
            string catalogSerialized = JsonSerializer.Serialize(catalog);
            var catalogHttpContent = new StringContent(catalogSerialized, Encoding.UTF8, "application/json");
            var createCatalog = await httpClient.PostAsync("http://localhost:5138/api/Catalog", catalogHttpContent);
            if (!createCatalog.IsSuccessStatusCode)
                throw new InvalidOperationException("error when creating new catalog");
        }

        return Redirect("/Index");
    }
}
