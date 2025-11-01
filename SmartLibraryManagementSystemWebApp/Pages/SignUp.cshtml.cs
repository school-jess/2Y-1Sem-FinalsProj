using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class SignUpModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPostAsync()
        {
            // todo post new user
            return Redirect("/Login");
        }
    }
}
