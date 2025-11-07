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
            public bool IsEducator { get; set; }
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
            if (!ModelState.IsValid) return Page();
            if (Input.Password != Input.ConfirmPassword) return Page();
            using (var httpClient = new HttpClient())
            {
                string apiLink = "http://localhost:5138/api/";
                if (Input.IsEducator) apiLink += "Faculty";
                else apiLink += $"Student";
                HttpResponseMessage createFacultyStudent;
                FacultyCreationDto faculty;
                StudentCreationDto student;
                HttpResponseMessage createUser;
                UserCreationDto user;
                if (Input.IsEducator)
                {
                    faculty = new FacultyCreationDto
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
                    createFacultyStudent = await httpClient.PostAsync(apiLink, facultyHttpCont);
                    var getFaculty = await httpClient.GetAsync($"http://localhost:5138/api/Faculty/{Input.Name}");
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
                    student = new StudentCreationDto
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
                    createFacultyStudent = await httpClient.PostAsync(apiLink, studentHttpCont);
                    var getStudent = await httpClient.GetAsync($"http://localhost:5138/api/Student/{Input.Name}");
                    if (!getStudent.IsSuccessStatusCode) return Page();
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
                }
                if (!(createFacultyStudent.IsSuccessStatusCode && createUser.IsSuccessStatusCode)) return new StatusCodeResult(500);
            }
            return Redirect("/LogIn");
        }
    }
}
