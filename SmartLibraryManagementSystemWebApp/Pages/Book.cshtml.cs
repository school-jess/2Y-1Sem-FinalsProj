using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace MyApp.Namespace
{
    public class BookModel : PageModel
    {
        public BookGet1Dto Book { get; set; }
        public async Task OnGetAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var resp = await httpClient.GetAsync($"http://localhost:5138/api/Book/{id}");
                if (resp.IsSuccessStatusCode)
                {
                    var respCont = await resp.Content.ReadAsStringAsync();
                    Book = JsonSerializer.Deserialize<BookGet1Dto>(respCont);
                } // todo return not found page
            }
        }
    }
}
