using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages
{
    public class UpdateBookModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            public string BookName { get; set; }
            public string BookAuthor { get; set; }
            public DateTime ReleaseDate { get; set; }
            public string Synopsis { get; set; }
            public string ClassificationId { get; set; }
            public int Copies { get; set; }
            public string Genre { get; set; }
        }

        public UpdateBookModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult OnGet() => NotFound();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) throw new InvalidOperationException("invalid input");
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
            if (HttpContext.Session.GetString("IsAdmin") != "true") return NotFound();
            var bookId = 0;
            using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
            {
                var getCatalog = await httpClient.GetAsync($"http://localhost:5138/api/Catalog/{Input.ClassificationId}");
                if (!getCatalog.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get catalog");
                string getCatalogContents = await getCatalog.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                Console.WriteLine(getCatalogContents);
                CatalogGet1Dto catalog = JsonSerializer.Deserialize<CatalogGet1Dto>(getCatalogContents, options);
                bookId = catalog.Book.BookId;
                BookUpdateDto bookToUpdate = new BookUpdateDto(
                    catalog.Book.BookId,
                    Input.BookName,
                    Input.BookAuthor,
                    Input.ReleaseDate,
                    Input.Synopsis,
                    catalog.Book.BookImgPath);
                string bookToUpdateSerialized = JsonSerializer.Serialize(bookToUpdate);
                var bookToUpdateHttpCont = new StringContent(bookToUpdateSerialized, Encoding.UTF8, "application/json");
                var updateBook = await httpClient.PutAsync($"http://localhost:5138/api/Book/{catalog.Book.BookId}", bookToUpdateHttpCont);
                if (!updateBook.IsSuccessStatusCode) throw new InvalidOperationException("couldn't update book");
                CatalogUpdateDto catalogToUpdate = new CatalogUpdateDto(
                    catalog.CatalogId,
                    catalog.Book.BookId,
                    Input.Copies,
                    Input.Genre,
                    Input.ClassificationId,
                    catalog.CopiesBorrowed);
                string catalogToUpdateSerialized = JsonSerializer.Serialize(catalogToUpdate);
                var catalogToUpdateHttpCont = new StringContent(catalogToUpdateSerialized, Encoding.UTF8, "application/json");
                var updateCatalog = await httpClient.PutAsync($"http://localhost:5138/api/Catalog/{catalogToUpdate.CatalogId}", catalogToUpdateHttpCont);
                if (!updateCatalog.IsSuccessStatusCode) throw new InvalidOperationException("couldn't update catalog");
            }
            return Redirect($"/Book/{bookId}");
        }
    }
}
