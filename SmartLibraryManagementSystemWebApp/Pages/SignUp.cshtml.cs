using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using System.Text.Json;
using System.Text;

namespace MyApp.Namespace
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public EducatorInputModel FacultyInput { get; set; }
        [BindProperty]
        public StudentInputModel StudentInput { get; set; }

        public class InputModel
        {
            [StringLength(50)]
            public string Name { get; set; }
            public string Department { get; set; }
            public string Course { get; set; }
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
            public bool IsEducator { get; set; } = true;
        }

        public class EducatorInputModel
        {
            public string Subject { get; set; }
        }

        public class StudentInputModel
        {
            public int Grade { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // todo handle authentication in the web api instead of here 
            if (!ModelState.IsValid) return Page();
            if (Input.Password != Input.ConfirmPassword) return Page();
            using (var httpClient = new HttpClient())
            {
                string apiLink = "http://localhost:5138/api/";
                if (Input.IsEducator) apiLink += "Faculty";
                else apiLink += $"Student";
                HttpResponseMessage resp;
                if (Input.IsEducator)
                {
                    FacultyCreationDto faculty = new FacultyCreationDto
                    {
                        Course = Input.Course,
                        Department = Input.Department,
                        Email = Input.Email,
                        FacultyName = Input.Name,
                        IsLoggedIn = false,
                        Password = Input.Password,
                        Subject = FacultyInput.Subject,
                    };
                    string facultySerialized = JsonSerializer.Serialize(faculty);
                    var facultyHttpCont = new StringContent(facultySerialized, Encoding.UTF8, "application/json");
                    resp = await httpClient.PostAsync(apiLink, facultyHttpCont);

                }
                else
                {
                    StudentCreationDto student = new StudentCreationDto
                    {
                        Course = Input.Course,
                        Department = Input.Department,
                        Email = Input.Email,
                        StudentName = Input.Name,
                        IsLoggedIn = false,
                        Password = Input.Password,
                        Grade = StudentInput.Grade,
                    };
                    string studentSerialized = JsonSerializer.Serialize(student);
                    var studentHttpCont = new StringContent(studentSerialized, Encoding.UTF8, "application/json");
                    resp = await httpClient.PostAsync(apiLink, studentHttpCont);
                }
                if (!resp.IsSuccessStatusCode) return Page();
            }
            return Redirect("/Login");
        }
    }
}
