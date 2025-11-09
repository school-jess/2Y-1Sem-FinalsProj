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

        public class InputModel
        {
            [StringLength(50)]
            public string Name { get; set; }
            [StringLength(10)]
            public string Department { get; set; }
            [StringLength(2)]
            public string Course { get; set; }
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
            public bool IsEducator { get; set; }
            [StringLength(20)]
            public string? Subject { get; set; }
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
                else apiLink += $"Student";
                HttpResponseMessage createFacultyStudent;
                HttpResponseMessage createUser;
                UserCreationDto user;
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
                        Subject = Input.Subject,
                    };
                    string facultySerialized = JsonSerializer.Serialize(faculty);
                    var facultyHttpCont = new StringContent(facultySerialized, Encoding.UTF8, "application/json");
                    createFacultyStudent = await httpClient.PostAsync(apiLink, facultyHttpCont);
                    var getFaculty = await httpClient.GetAsync($"http://localhost:5138/api/Faculty/{Input.Email}");
                    if (!getFaculty.IsSuccessStatusCode) return Page();
                    var getFacultyContent = await getFaculty.Content.ReadAsStringAsync();
                    var insertedFaculty = JsonSerializer.Deserialize<FacultyGet1Dto>(getFacultyContent);
                    user = new UserCreationDto
                    {
                        FacultyId = insertedFaculty.FacultyId,
                        HasFine = false,
                        HasLoan = false,
                        IsAdmin = false,
                        IsFaculty = true,
                        StudentId = 0,
                        UserName = faculty.FacultyName,
                    };
                    string userSerialized = JsonSerializer.Serialize(user);
                    var userHttpCont = new StringContent(userSerialized, Encoding.UTF8, "application/json");
                    createUser = await httpClient.PostAsync("http://localhost:5138/api/User/", userHttpCont);
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
                        Grade = Input.Grade ?? 0,
                    };
                    string studentSerialized = JsonSerializer.Serialize(student);
                    var studentHttpCont = new StringContent(studentSerialized, Encoding.UTF8, "application/json");
                    createFacultyStudent = await httpClient.PostAsync(apiLink, studentHttpCont);
                    if (!createFacultyStudent.IsSuccessStatusCode) return new StatusCodeResult(500);
                    var getStudent = await httpClient.GetAsync($"http://localhost:5138/api/Student/{Input.Email}");
                    if (!getStudent.IsSuccessStatusCode) return new StatusCodeResult(500);
                    var getStudentContent = await getStudent.Content.ReadAsStringAsync();
                    var insertedStudent = JsonSerializer.Deserialize<StudentGet1Dto>(getStudentContent);
                    user = new UserCreationDto
                    {
                        FacultyId = 0,
                        HasFine = false,
                        HasLoan = false,
                        IsAdmin = false,
                        IsFaculty = false,
                        StudentId = insertedStudent.StudentId,
                        UserName = student.StudentName,
                    };
                    string userSerialized = JsonSerializer.Serialize(user);
                    var userHttpCont = new StringContent(userSerialized, Encoding.UTF8, "application/json");
                    createUser = await httpClient.PostAsync("http://localhost:5138/api/User/", userHttpCont);
                    if (!createUser.IsSuccessStatusCode) return new StatusCodeResult(500);
                }
            }
            return Redirect("/LogIn");
        }
    }
}
