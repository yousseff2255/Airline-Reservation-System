using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.Common;
using System.Security.Principal;

namespace DatabaseProject.Pages
{
    public class SignInModel(DB db) : PageModel
    {

        public string ValidationMessage { get; set; }

        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Email { get; set; }
        public int Success { get; set; } = 0;

        public bool buttonPressed { get; set; } = false;

        

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
                DataTable dt = new DataTable();
            
           string role = db.gettheUserRole(Email, Password);

            buttonPressed = true;
            Success = db.CheckSignIN(Email, Password);
            if (Success is 1)
            {


                ValidationMessage = "Correct Credits ";
                //string role  = (db.gettheUserRole(Email, Password)).Rows[0]['role'];

                HttpContext.Session.SetString("email", Email);
                HttpContext.Session.SetString("password", Password);

                HttpContext.Session.SetString("role", role);
                if (role == "admin" || role=="staff")
                {
                    return RedirectToPage("/admin/dashboard");
                }

                return RedirectToPage("/Profile");

            }
            else
            {
                ValidationMessage = "Invalid email or Password ";
                return RedirectToPage("SignIn");
            }


        }


    }
}