using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class UserModel : PageModel
{
    public UserWithReservationsDto User { get; set; }
    public bool IsFaculty { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("UserId") != $"{id}") return NotFound();
        IsFaculty = HttpContext.Session.GetString("IsFaculty") == "true";
        using (var httpClient = new HttpClient())
        {
            var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{id}?withReservation=true");
            if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("error getting user");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getUserContent = await getUser.Content.ReadAsStringAsync();
            User = JsonSerializer.Deserialize<UserWithReservationsDto>(getUserContent, options);
            Console.WriteLine(User.Faculty);
        }
        return Page();
    }
}
