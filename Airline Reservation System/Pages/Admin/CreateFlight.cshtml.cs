using System.Data;
using Airline_Reservation_System.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Admin
{
    
    public class CreateFlightModel : PageModel
    {
        public DB db { get; set; }
        public DataTable airports { get; set; }
        public List<string> models { get; set; }
        [BindProperty]
        public Flight flight { get; set; }
        [BindProperty]
        static bool b { get; set; } = true;
        public bool bb { get; set; } = true;
        public CreateFlightModel(DB db)
        {
            airports = new DataTable();
            models = new List<string>();
            flight = new Flight();
            this.db = db;
           
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role").ToLower() == "admin")
            {
                bb = b;
                airports = db.GetAirports();
                models = db.GetModels();
                return Page();
             
            }
            else
            {
				return RedirectToPage("../index");
			}

        
           
        }
        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("password");
            return RedirectToPage("../signin");
        }

        public IActionResult OnPost() {
            
            b = db.AddFlight(flight);
           
            if(b==true)
                return RedirectToPage("Dashboard");
            return RedirectToPage();
        }
    }
}



