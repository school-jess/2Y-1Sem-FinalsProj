using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class AdminModel : PageModel
{
    public List<UserUpdateDto> Users { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        using (var httpClient = new HttpClient())
        {
            var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}?withReservation=false");
            if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("coudn't get user");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            string getUserCont = await getUser.Content.ReadAsStringAsync();
            UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserCont, options);
            if (!user.IsAdmin) return NotFound();
            var getUsers = await httpClient.GetAsync("http://localhost:5138/api/User");
            if (!getUsers.IsSuccessStatusCode) throw new InvalidOperationException("coudn't get users");
            string getUsersCont = await getUsers.Content.ReadAsStringAsync();
            Users = JsonSerializer.Deserialize<List<UserUpdateDto>>(getUsersCont, options);
        }
        return Page();
    }
}
