using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;

namespace MyApp.Namespace
{
    public class NewBookModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public DateTime ReleaseDate { get; set; }
            public string Synopsis { get; set; }
            public string ClassificationId { get; set; }
            public int Copies { get; set; }
            public string Genre { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
            string userName = HttpContext.Session.GetString("LogInName");
            using (var httpClient = new HttpClient())
            {
                var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{userName}"); // todo should not be faculty or student name to use
                if (!getUser.IsSuccessStatusCode) return new StatusCodeResult(500);
                var getUserCont = await getUser.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserGet1Dto>(getUserCont);
                if (!user.IsAdmin) return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            using (var httpClient = new HttpClient())
            {
                BookCreationDto book = new BookCreationDto
                {
                    Author = Input.Author,
                    BookName = Input.Name,
                    ReleaseDate = Input.ReleaseDate,
                    Synopsis = Input.Synopsis
                };
                string bookSerialized = JsonSerializer.Serialize(book);
                var bookHttpCont = new StringContent(bookSerialized, Encoding.UTF8, "application/json");
                var newBook = await httpClient.PostAsync("http://localhost:5138/api", bookHttpCont);
                var getBook = await httpClient.GetAsync($"http://localhost:5138/api/Book/{Input.Name}");
                if (!getBook.IsSuccessStatusCode) return new StatusCodeResult(500);
                var getBookContent = await getBook.Content.ReadAsStringAsync();
                var insertedBook = JsonSerializer.Deserialize<BookGet1Dto>(getBookContent);
                CatalogCreationDto catalog = new CatalogCreationDto
                {
                    BookId = insertedBook.BookId,
                    ClassificationId = Input.ClassificationId,
                    Copies = Input.Copies,
                    Genre = Input.Genre,
                };
                string catalogSerialized = JsonSerializer.Serialize(catalog);
                var catalogHttpContent = new StringContent(catalogSerialized, Encoding.UTF8, "application/json");
                var createCatalog = await httpClient.PostAsync("http://localhost:5138/api/Catalog/", catalogHttpContent);
                if (!newBook.IsSuccessStatusCode) return new StatusCodeResult(500);
            }
            return Redirect("/Index");
        }
    }
}
