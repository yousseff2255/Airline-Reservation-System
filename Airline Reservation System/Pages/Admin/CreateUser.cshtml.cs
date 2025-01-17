using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Airline_Reservation_System.Pages.Admin
{
    public class CreateUser : PageModel
    {
        public DB db { get; set; }
        [Required, BindProperty]
        public string Role { get; set; }

        [Required, BindProperty]
        public string Username { get; set; }

        [MinLength(8), Required, BindProperty]
        public string Password { get; set; }



        public CreateUser(DB db)
        {
            this.db = db;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role").ToLower() == "admin")
            {
        
                return Page();

            }
            else
            {
                return RedirectToPage("../index");
            }

        }

        public void OnPost()
        {
            db.CreateUser(Username, Password, Role);

        }
        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("password");
            return RedirectToPage("../signin");
        }


    }
}