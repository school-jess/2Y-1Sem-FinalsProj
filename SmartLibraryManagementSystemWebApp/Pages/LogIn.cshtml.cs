using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using System.Text.Json;
using System.Text;

namespace MyApp.Namespace
{
    public class LogInModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            public bool IsEducator { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            using (var httpClient = new HttpClient())
            {
                // todo handle authentication in the web api instead of here 
                string apiLink = "http://localhost:5138/api/";
                if (Input.IsEducator) apiLink += $"Faculty/{Input.Email}";
                else apiLink += $"Student/{Input.Email}";
                var resp = await httpClient.GetAsync(apiLink);
                if (resp.IsSuccessStatusCode)
                {
                    var respContent = await resp.Content.ReadAsStringAsync();
                    if (Input.IsEducator)
                    {
                        FacultyGet1Dto faculty = JsonSerializer.Deserialize<FacultyGet1Dto>(respContent);
                        if (faculty.Password == Input.Password)
                        {
                            HttpContext.Session.SetString("IsLoggedIn", "true");
                            HttpContext.Session.SetString("LogInName", faculty.FacultyName);
                            HttpContext.Session.SetString("UserId", $"{faculty.FacultyId}");
                            faculty.IsLoggedIn = true;
                            string facultySerialized = JsonSerializer.Serialize(faculty);
                            var facultyHttpCont = new StringContent(facultySerialized, Encoding.UTF8, "application/json");
                            var setLoginStat = await httpClient.PutAsync($"http://localhost:5138/api/Faculty/{faculty.FacultyId}", facultyHttpCont);
                            if (!setLoginStat.IsSuccessStatusCode) return Page(); // todo tell user something went wrong on our end
                        }
                        else return Page(); // todo return login failed
                    }
                    else
                    {
                        StudentGet1Dto student = JsonSerializer.Deserialize<StudentGet1Dto>(respContent);
                        if (student.Password == Input.Password)
                        {
                            HttpContext.Session.SetString("IsLoggedIn", "true");
                            HttpContext.Session.SetString("LogInName", student.StudentName);
                            HttpContext.Session.SetString("UserId", $"{student.StudentId}");
                            student.IsLoggedIn = true;
                            string studentSerialized = JsonSerializer.Serialize(student);
                            var studentHttpCont = new StringContent(studentSerialized, Encoding.UTF8, "application/json");
                            var setLoginStat = await httpClient.PutAsync($"http://localhost:5138/api/Student/{student.StudentId}", studentHttpCont);
                            if (!setLoginStat.IsSuccessStatusCode) return Page();
                        }
                        else return Page(); // todo return login failed
                    }
                }
                else return Page();
            }
            return Redirect("/Index");
        }
    }
}
