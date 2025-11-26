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
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWebHostEnvironment _environment;

    public class InputModel
    {
        [StringLength(30)] public string Name { get; set; }
        [StringLength(50)] public string Author { get; set; }
        [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }
        public string Synopsis { get; set; }
        [StringLength(10)] public string ClassificationId { get; set; }
        public int Copies { get; set; }
        [StringLength(10)] public string Genre { get; set; }
        public IFormFile BookImg { get; set; }
    }

    public NewBookModel(IHttpClientFactory httpClientFactory, IWebHostEnvironment environment)
    {
        _httpClientFactory = httpClientFactory;
        _environment = environment;
    }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("IsAdmin") != "true") return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("IsAdmin") != "true") return NotFound();
        if (!ModelState.IsValid) return Page();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);
            var uniqueFileName = Guid.NewGuid() + "_" + Input.BookImg.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Input.BookImg.CopyToAsync(fileStream);
            }

            string uploadPath = "/uploads/" + uniqueFileName;
            BookCreationDto book =
                new BookCreationDto(
                    Input.Name,
                    Input.Author,
                    Input.ReleaseDate,
                    Input.Synopsis,
                    uploadPath);
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
            CatalogCreationDto catalog = new CatalogCreationDto(
                insertedBook.BookId,
                Input.Copies,
                Input.Genre,
                Input.ClassificationId,
                0);
            string catalogSerialized = JsonSerializer.Serialize(catalog);
            var catalogHttpContent = new StringContent(catalogSerialized, Encoding.UTF8, "application/json");
            var createCatalog = await httpClient.PostAsync("http://localhost:5138/api/Catalog", catalogHttpContent);
            if (!createCatalog.IsSuccessStatusCode)
                throw new InvalidOperationException("error when creating new catalog");
        }

        return Redirect("/Index");
    }
}
