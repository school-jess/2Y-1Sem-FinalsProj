using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class BookModel : PageModel
{
    public BookGet1Dto Book { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        using (var httpClient = new HttpClient())
        {
            var resp = await httpClient.GetAsync($"http://localhost:5138/api/Book/{id}");
            if (resp.IsSuccessStatusCode)
            {
                var respCont = await resp.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Book = JsonSerializer.Deserialize<BookGet1Dto>(respCont, options);
            }
            else return NotFound();
        }

        return Page();
    }
}

