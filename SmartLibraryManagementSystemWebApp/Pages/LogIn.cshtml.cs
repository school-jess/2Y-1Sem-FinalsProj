using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using System.Text.Json;
using System.Text;
using Isopoh.Cryptography.Argon2;

namespace SmartLibraryManagementSystemWebApp.Pages;

public class LogInModel : PageModel
{
    [BindProperty] public InputModel Input { get; set; }

    public class InputModel
    {
        [EmailAddress] public string Email { get; set; }
        [DataType(DataType.Password)] public string Password { get; set; }
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
            string apiLink = "http://localhost:5138/api/";
            if (Input.IsEducator) apiLink += $"Faculty/{Input.Email}";
            else apiLink += $"Student/{Input.Email}";
            var resp = await httpClient.GetAsync(apiLink);
            string respContent;
            if (resp.IsSuccessStatusCode)
            {
                respContent = await resp.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                if (Input.IsEducator)
                {
                    FacultyGet1Dto faculty = JsonSerializer.Deserialize<FacultyGet1Dto>(respContent, options);
                    if (Argon2.Verify(faculty.Password, Input.Password))
                    {
                        HttpContext.Session.SetString("IsLoggedIn", "true");
                        HttpContext.Session.SetString("LogInName", faculty.FacultyName);
                        HttpContext.Session.SetString("UserId", $"{faculty.User.UserId}");
                        HttpContext.Session.SetString("IsEducator", "true");
                        if (faculty.User.IsAdmin) HttpContext.Session.SetString("IsAdmin", "true");
                        else HttpContext.Session.SetString("IsAdmin", "false");
                        faculty.IsLoggedIn = true;
                        string facultySerialized = JsonSerializer.Serialize(faculty);
                        var facultyHttpCont =
                            new StringContent(facultySerialized, Encoding.UTF8, "application/json");
                        var setLoginStat =
                            await httpClient.PutAsync($"http://localhost:5138/api/Faculty/{faculty.FacultyId}",
                                facultyHttpCont);
                        if (!setLoginStat.IsSuccessStatusCode)
                            throw new InvalidOperationException("error setting login status");
                    }
                    else throw new InvalidOperationException("password dont match"); // todo handle with more care
                }
                else
                {
                    respContent = await resp.Content.ReadAsStringAsync();
                    StudentGet1Dto student = JsonSerializer.Deserialize<StudentGet1Dto>(respContent, options);
                    if (Argon2.Verify(student.Password, Input.Password))
                    {
                        HttpContext.Session.SetString("IsLoggedIn", "true");
                        HttpContext.Session.SetString("LogInName", student.StudentName);
                        HttpContext.Session.SetString("UserId", $"{student.User.UserId}");
                        HttpContext.Session.SetString("IsEducator", "false");
                        if (student.User.IsAdmin) HttpContext.Session.SetString("IsAdmin", "true");
                        else HttpContext.Session.SetString("IsAdmin", "false");
                        student.IsLoggedIn = true;
                        string studentSerialized = JsonSerializer.Serialize(student);
                        var studentHttpCont =
                            new StringContent(studentSerialized, Encoding.UTF8, "application/json");
                        var setLoginStat =
                            await httpClient.PutAsync($"http://localhost:5138/api/Student/{student.StudentId}",
                                studentHttpCont);
                        if (!setLoginStat.IsSuccessStatusCode)
                            throw new InvalidOperationException("error setting login status");
                    }
                    else return Page(); // todo handle with more care
                }
            }
            else throw new InvalidOperationException("couldn't find user");
        }

        return Redirect("/Index");
    }
}
