using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using SmartLibraryManagementSystemClassLibrary.Dtos;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class LogOutModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LogOutModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
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
                FacultyUpdateDto facultyToLogout = new FacultyUpdateDto(
                    user.Faculty.FacultyId,
                    user.Faculty.FacultyName,
                    user.Faculty.Department,
                    user.Faculty.Subject,
                    user.Faculty.Course,
                    user.Faculty.Email,
                    user.Faculty.Password,
                    false,
                    user.Faculty.UserId,
                    user.Faculty.ProfileImgPath);
                var facultyToLogoutSerialized = JsonSerializer.Serialize(facultyToLogout);
                var facultyToLogoutHttpCont =
                    new StringContent(facultyToLogoutSerialized, Encoding.UTF8, "application/json");
                var setLogInStat =
                    await httpClient.PutAsync($"http://localhost:5138/api/Faculty/{facultyToLogout.FacultyId}",
                        facultyToLogoutHttpCont);
                if (!setLogInStat.IsSuccessStatusCode)
                    throw new InvalidOperationException("couldn't faculty set log in status");
            }
            else
            {
                StudentUpdateDto studentToLogout = new StudentUpdateDto(
                    user.Student.StudentId,
                    user.Student.StudentName,
                    user.Student.Department,
                    user.Student.Course,
                    user.Student.Grade,
                    user.Student.Email,
                    user.Student.Password,
                    false,
                    user.Student.UserId,
                    user.Student.ProfileImgPath);
                var studentToLogoutSerialized = JsonSerializer.Serialize(studentToLogout);
                var studentToLogoutHttpCont =
                    new StringContent(studentToLogoutSerialized, Encoding.UTF8, "application/json");
                var setLogInStat =
                    await httpClient.PutAsync($"http://localhost:5138/api/Student/{studentToLogout.StudentId}",
                        studentToLogoutHttpCont);
                if (!setLogInStat.IsSuccessStatusCode)
                    throw new InvalidOperationException("couldn't set student log in status");
            }
        }

        HttpContext.Session.SetString("IsLoggedIn", "false");
        HttpContext.Session.SetString("LogInName", "");
        HttpContext.Session.SetString("UserId", "");
        HttpContext.Session.SetString("IsEducator", "");
        return Redirect("/Index");
    }
}
