using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class UserModel : PageModel
{
    public UserWithReservationsDto User { get; set; }
    public bool IsFaculty { get; set; }
    private readonly IHttpClientFactory _httpClientFactory;

    public UserModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("UserId") != $"{id}") return NotFound();
        IsFaculty = HttpContext.Session.GetString("IsEducator") == "true";
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{id}?withReservation=true");
            if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("error getting user");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var getUserContent = await getUser.Content.ReadAsStringAsync();
            User = JsonSerializer.Deserialize<UserWithReservationsDto>(getUserContent, options);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var deleteUser =
                await httpClient.DeleteAsync(
                    $"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}");
            if (!deleteUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't delete user");
        }

        return Page();
    }
}
