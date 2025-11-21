using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Isopoh.Cryptography.Argon2;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class UserModel : PageModel
{
    public UserWithReservationsDto User { get; set; }
    public bool IsFaculty { get; set; }
    [BindProperty] public UpdateUserInfo Input { get; set; }
    private readonly IHttpClientFactory _httpClientFactory;

    public class UpdateUserInfo
    {
        public string UserName { get; set; }
        public string UserCourse { get; set; }
        public string UserDepartment { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string? UserSubject { get; set; }
        public int UserGrade { get; set; }
        public int UserIsUpdateing { get; set; }
    }

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        if (!ModelState.IsValid) return Page();
        if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
        using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
        {
            if (Input.UserIsUpdateing == 1)
            {
                var getUser = await httpClient.GetAsync($"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}?withReservation=false");
                if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get user");
                var getUserContent = await getUser.Content.ReadAsStringAsync();
                UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserContent);
                if (HttpContext.Session.GetString("IsEducator") == "true")
                {
                    FacultyUpdateDto facultyToUpdate = new FacultyUpdateDto(
                        user.Faculty.FacultyId,
                        Input.UserName,
                        Input.UserDepartment,
                        "",
                        Input.UserCourse,
                        Input.UserEmail,
                        Argon2.Hash(Input.UserPassword),
                        true,
                        user.Faculty.UserId);
                    string facultyToUpdateSerialized = JsonSerializer.Serialize(facultyToUpdate);
                    var facultyToUpdateHttpCont =
                        new StringContent(facultyToUpdateSerialized, Encoding.UTF8, "application/json");
                    var updateFaculty =
                        await httpClient.PutAsync(
                            $"http://localhost:5138/api/Faculty/{user.Faculty.FacultyId}", facultyToUpdateHttpCont);
                    if (!updateFaculty.IsSuccessStatusCode)
                        throw new InvalidOperationException("couldn't update faculty");
                }
                else
                {
                    StudentUpdateDto studentToUpdate = new StudentUpdateDto(
                        user.Student.StudentId,
                        Input.UserName,
                        Input.UserDepartment,
                        Input.UserCourse,
                        0,
                        Input.UserEmail,
                        Argon2.Hash(Input.UserPassword),
                        true,
                        user.Student.UserId);
                    string studentToUpdateSerialized = JsonSerializer.Serialize(studentToUpdate);
                    var studentToUpdateHttpCont =
                        new StringContent(studentToUpdateSerialized, Encoding.UTF8, "application/json");
                    var updateStudent = await httpClient.PutAsync(
                        $"http://localhost:5138/api/Student/{user.Student.StudentId}", studentToUpdateHttpCont);
                    if (!updateStudent.IsSuccessStatusCode)
                        throw new InvalidOperationException("couldn't update student");
                }
            }
            else
            {
                var deleteUser =
                    await httpClient.DeleteAsync(
                        $"http://localhost:5138/api/User/{HttpContext.Session.GetString("UserId")}");
                if (!deleteUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't delete user");
            }
        }

        return Redirect("/Index");
    }
}
