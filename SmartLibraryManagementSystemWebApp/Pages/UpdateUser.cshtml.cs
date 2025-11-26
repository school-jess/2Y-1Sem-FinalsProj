using Microsoft.AspNetCore.Mvc;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using Isopoh.Cryptography.Argon2;

namespace SmartLibraryManagementSystemWebApp.Pages
{
    public class UpdateUserModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty] public InputModel Input { get; set; }
        private readonly IWebHostEnvironment _environment;

        public class InputModel
        {
            public string UserName { get; set; }
            public string UserDepartment { get; set; }
            public string UserCourse { get; set; }
            public string UserEmail { get; set; }
            public string? UserPassword { get; set; }
            public string? UserSubject { get; set; }
            public int? UserGrade { get; set; }
            public IFormFile ProfileImg { get; set; }
        }

        public UpdateUserModel(IHttpClientFactory httpClientFactory, IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        public IActionResult OnGet() => NotFound();

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true") return NotFound();
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            using (var httpClient = _httpClientFactory.CreateClient("LibraryApi"))
            {
                var getUser = await httpClient.GetAsync(
                    $"http://localhost:5138/api/User/{userId}?withReservation=false");
                if (!getUser.IsSuccessStatusCode) throw new InvalidOperationException("couldn't get user");
                var getUserContent = await getUser.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                UserGet1Dto user = JsonSerializer.Deserialize<UserGet1Dto>(getUserContent, options);
                string password = "";

                if (HttpContext.Session.GetString("IsEducator") == "true")
                {
                    if (Input.UserPassword == null) password = user.Faculty.Password;
                    else password = Argon2.Hash(Input.UserPassword);
                    var uploadPath = Path.Join(_environment.WebRootPath, user.Faculty.ProfileImgPath);
                    using (var fileStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
                    {
                        await Input.ProfileImg.CopyToAsync(fileStream);
                    }

                    FacultyUpdateDto facultyToUpdate = new FacultyUpdateDto(
                        user.Faculty.FacultyId,
                        Input.UserName,
                        Input.UserDepartment,
                        Input.UserSubject,
                        Input.UserCourse,
                        Input.UserEmail,
                        password,
                        true,
                        user.Faculty.UserId,
                        user.Faculty.ProfileImgPath);
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
                    if (Input.UserPassword == null) password = user.Student.Password;
                    else password = Argon2.Hash(Input.UserPassword);
                    var uploadPath = Path.Join(_environment.WebRootPath, user.Student.ProfileImgPath);
                    using (var fileStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
                    {
                        await Input.ProfileImg.CopyToAsync(fileStream);
                    }

                    StudentUpdateDto studentToUpdate = new StudentUpdateDto(
                        user.Student.StudentId,
                        Input.UserName,
                        Input.UserDepartment,
                        Input.UserCourse,
                        Input.UserGrade ?? 0,
                        Input.UserEmail,
                        password,
                        true,
                        user.Student.UserId,
                        user.Student.ProfileImgPath);
                    string studentToUpdateSerialized = JsonSerializer.Serialize(studentToUpdate);
                    var studentToUpdateHttpCont =
                        new StringContent(studentToUpdateSerialized, Encoding.UTF8, "application/json");
                    var updateStudent = await httpClient.PutAsync(
                        $"http://localhost:5138/api/Student/{user.Student.StudentId}", studentToUpdateHttpCont);
                    if (!updateStudent.IsSuccessStatusCode)
                        throw new InvalidOperationException("couldn't update student");
                }
            }

            return Redirect($"/User/{userId}");
        }
    }
}
