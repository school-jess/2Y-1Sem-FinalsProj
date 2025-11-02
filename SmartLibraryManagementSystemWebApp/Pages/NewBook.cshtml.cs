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
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
            string userName = HttpContext.Session.GetString("LogInName");
            using (var httpClient = new HttpClient())
            {
                var resp = httpClient.GetAsync($"http://localhost:5138/api/User/{userName}");
                if (HttpContext.Session.GetString("IsEducator") != "true")
                {
                }
                else
                {
                }
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
                var resp = await httpClient.PostAsync("http://localhost:5138/api", bookHttpCont);
                if (!resp.IsSuccessStatusCode) return Page();
            }
            return Redirect("/Index");
        }
    }
}
