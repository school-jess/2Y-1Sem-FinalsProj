using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
            [StringLength(30)]
            public string Name { get; set; }
            [StringLength(50)]
            public string Author { get; set; }
            [DataType(DataType.Date)]
            public DateTime ReleaseDate { get; set; }
            public string Synopsis { get; set; }
            [StringLength(10)]
            public string ClassificationId { get; set; }
            public int Copies { get; set; }
            [StringLength(10)]
            public string Genre { get; set; }
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
                BookCreationDto book = new BookCreationDto
                {
                    Author = Input.Author,
                    BookName = Input.Name,
                    ReleaseDate = Input.ReleaseDate,
                    Synopsis = Input.Synopsis
                };
                string bookSerialized = JsonSerializer.Serialize(book);
                var bookHttpCont = new StringContent(bookSerialized, Encoding.UTF8, "application/json");
                var newBook = await httpClient.PostAsync("http://localhost:5138/api/Book", bookHttpCont);
                if (!newBook.IsSuccessStatusCode) throw new InvalidOperationException("error when creating new book");
                var getBook = await httpClient.GetAsync($"http://localhost:5138/api/Book/{Input.Name}");
                if (!getBook.IsSuccessStatusCode) throw new InvalidOperationException("error when getting new book");
                var getBookContent = await getBook.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var insertedBook = JsonSerializer.Deserialize<BookGet1Dto>(getBookContent, options);
                CatalogCreationDto catalog = new CatalogCreationDto
                {
                    BookId = insertedBook.BookId,
                    ClassificationId = Input.ClassificationId,
                    Copies = Input.Copies,
                    Genre = Input.Genre,
                };
                string catalogSerialized = JsonSerializer.Serialize(catalog);
                var catalogHttpContent = new StringContent(catalogSerialized, Encoding.UTF8, "application/json");
                Console.WriteLine("hello");
                var createCatalog = await httpClient.PostAsync("http://localhost:5138/api/Catalog", catalogHttpContent);
                if (!createCatalog.IsSuccessStatusCode) throw new InvalidOperationException("error when creating new catalog");
            }
            return Redirect("/Index");
        }
    }
}
