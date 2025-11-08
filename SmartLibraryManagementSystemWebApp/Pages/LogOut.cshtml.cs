using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace MyApp.Namespace
{
    public class LogOutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
            using (var httpClient = new HttpClient())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string userId = HttpContext.Session.GetString("UserId");
                var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{userId}");
                if (!getUser.IsSuccessStatusCode) return new StatusCodeResult(500);
                var getUserCont = await getUser.Content.ReadAsStringAsync();
                UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserCont, options);
                if (HttpContext.Session.GetString("IsEducator") == "true")
                {
                    user.Faculty.IsLoggedIn = false;
                    var facultySerialized = JsonSerializer.Serialize(user.Faculty);
                    var facultyHttpCont = new StringContent(facultySerialized, Encoding.UTF8, "application/json");
                    var setLogInStat =
                        await httpClient.PutAsync($"http://localhost:5138/api/Faculty/{user.Faculty.FacultyId}",
                            facultyHttpCont);
                    if (!setLogInStat.IsSuccessStatusCode) return new StatusCodeResult(500);
                }
                else
                {
                    user.Student.IsLoggedIn = false;
                    var studentSerialized = JsonSerializer.Serialize(user.Student);
                    var studentHttpCont = new StringContent(studentSerialized, Encoding.UTF8, "application/json");
                    var setLogInStat =
                        await httpClient.PutAsync($"http://localhost:5138/api/Student/{user.Student.StudentId}",
                            studentHttpCont);
                    if (!setLogInStat.IsSuccessStatusCode) return new StatusCodeResult(500);
                }
            }

            HttpContext.Session.SetString("IsLoggedIn", "false");
            HttpContext.Session.SetString("LogInName", "");
            HttpContext.Session.SetString("UserId", "");
            HttpContext.Session.SetString("IsEducator", "");
            return Redirect("/Index");
        }
    }
}
