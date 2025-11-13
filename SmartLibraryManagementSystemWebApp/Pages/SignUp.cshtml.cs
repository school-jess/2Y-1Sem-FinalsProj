using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using System.Text.Json;
using System.Text;
using Isopoh.Cryptography.Argon2;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class SignUpModel : PageModel
{
    [BindProperty] public InputModel Input { get; set; }

    public class InputModel
    {
        [StringLength(50)] public string Name { get; set; }
        [StringLength(10)] public string Department { get; set; }
        [StringLength(2)] public string Course { get; set; }
        [EmailAddress] public string Email { get; set; }
        [DataType(DataType.Password)] public string Password { get; set; }
        [DataType(DataType.Password)] public string ConfirmPassword { get; set; }
        public bool IsEducator { get; set; }
        [StringLength(20)] public string? Subject { get; set; }
        public int? Grade { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        if (Input.Password != Input.ConfirmPassword) return Page();
        using (var httpClient = new HttpClient())
        {
            string apiLink = "http://localhost:5138/api/";
            if (Input.IsEducator) apiLink += "Faculty";
            else apiLink += "Student";
            UserCreationDto user = new UserCreationDto(Input.Name, false, false, false);
            string userSerialized = JsonSerializer.Serialize(user);
            var userHttpCont = new StringContent(userSerialized, Encoding.UTF8, "application/json");
            HttpResponseMessage createUser =
                await httpClient.PostAsync("http://localhost:5138/api/User", userHttpCont);
            if (!createUser.IsSuccessStatusCode)
                throw new InvalidOperationException("error when creating new user");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var newUser =
                JsonSerializer.Deserialize<UserUpdateDto>(await createUser.Content.ReadAsStringAsync(), options);
            if (Input.IsEducator)
            {
                FacultyCreationDto faculty = new FacultyCreationDto(Input.Name, Input.Department, Input.Subject,
                    Input.Course, Input.Email, Argon2.Hash(Input.Password), false, newUser.UserId);
                string facultySerialized = JsonSerializer.Serialize(faculty);
                var facultyHttpCont = new StringContent(facultySerialized, Encoding.UTF8, "application/json");
                HttpResponseMessage createFaculty = await httpClient.PostAsync(apiLink, facultyHttpCont);
                if (!createFaculty.IsSuccessStatusCode)
                    throw new InvalidOperationException("error when creating new faculty");
            }
            else
            {
                StudentCreationDto student = new StudentCreationDto(Input.Name, Input.Department, Input.Course,
                    Input.Grade ?? 0, Input.Email, Argon2.Hash(Input.Password), false, newUser.UserId);
                string studentSerialized = JsonSerializer.Serialize(student);
                var studentHttpCont = new StringContent(studentSerialized, Encoding.UTF8, "application/json");
                HttpResponseMessage createStudent = await httpClient.PostAsync(apiLink, studentHttpCont);
                if (!createStudent.IsSuccessStatusCode)
                    throw new InvalidOperationException("error when creating new student");
            }
        }

        return Redirect("/LogIn");
    }
}
