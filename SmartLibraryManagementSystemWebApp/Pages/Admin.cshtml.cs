using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class AdminModel : PageModel
{
    public List<UserUpdateDto> Users { get; set; }
    [BindProperty] public InputModel Input { get; set; }
    private readonly IHttpClientFactory _httpClientFactory;

    public class InputModel
    {
        public int UserId { get; set; }
        public int IsDelete { get; set; }
        public string UserName { get; set; }
        public bool HasFine { get; set; }
        public bool HasLoan { get; set; }
        public bool IsAdmin { get; set; }
    }

    public AdminModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("IsAdmin") != "true") return NotFound();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            var getUser = await httpClient.GetAsync(
                $"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}?withReservation=false");
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (HttpContext.Session.GetString("IsAdmin") != "true") return NotFound();
        if (!ModelState.IsValid) return Page();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            if (Input.IsDelete == 1)
            {
                var deleteUser = await httpClient.DeleteAsync($"http://localhost:5138/api/User/{Input.UserId}");
                if (!deleteUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't delete user");
            }
            else
            {
                UserUpdateDto userToUpdate = new UserUpdateDto(Input.UserId, Input.UserName, Input.HasFine,
                    Input.HasLoan, Input.IsAdmin);
                string userToUpdateSerialized = JsonSerializer.Serialize(userToUpdate);
                var userToUpdateHttpCont = new StringContent(userToUpdateSerialized, Encoding.UTF8, "application/json");
                var updateUser = await httpClient.PutAsync($"http://localhost:5138/api/User/{Input.UserId}",
                    userToUpdateHttpCont);
                if (!updateUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't update user");
                var getUsers = await httpClient.GetAsync("http://localhost:5138/api/User");
                if (!getUsers.IsSuccessStatusCode) throw new InvalidOperationException("coudn't get users");
                string getUsersCont = await getUsers.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Users = JsonSerializer.Deserialize<List<UserUpdateDto>>(getUsersCont, options);
            }
        }

        return Page();
    }
}
