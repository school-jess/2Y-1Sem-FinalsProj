using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using SmartLibraryManagementSystemClassLibrary.Dtos;

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
            [EmailAddress]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
            public bool IsEducator { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostAsync()
        {
            // todo handle authentication in the web api instead of here 
            if (!ModelState.IsValid) return Page();
            using (var httpClient = new HttpClient())
            {
                string apiLink = "http://localhost:5138/api/";
                if (Input.IsEducator) apiLink += "Faculty";
                else apiLink += $"Student";
                if (Input.IsEducator)
                {
                    FacultyCreationDto faculty = new FacultyCreationDto { };
                }
                else
                {
                    StudentCreationDto student = new StudentCreationDto { };
                }
                var resp = await httpClient.PostAsync(apiLink);
                if (resp.IsCompletedSuccessfully) { }
            }
            return Redirect("/Login");
        }
    }
}
