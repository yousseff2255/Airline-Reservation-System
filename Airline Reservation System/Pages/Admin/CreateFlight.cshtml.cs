using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Admin
{
    
    public class CreateFlightModel : PageModel
    {
        public DB db { get; set; }
        public List<string> airports { get; set; }
        public List<string> models { get; set; }
        [BindProperty]
        public Flight flight { get; set; }
        [BindProperty]
        static bool b { get; set; } = true;
        public bool bb { get; set; } = true;
        public CreateFlightModel(DB db)
        {
            airports = new List<string>();
            models = new List<string>();
            flight = new Flight();
            this.db = db;
           
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") == "passanger" || string.IsNullOrEmpty(HttpContext.Session.GetString("role")))
            {
                return RedirectToPage("/Index");
            }
            bb = b;
            airports = db.GetAirports();
            models = db.GetModels();
            return Page();
           
        }

        public IActionResult OnPost() {
            
            b = db.AddFlight(flight);
           
            if(b==true)
                return RedirectToPage("Dashboard");
            return RedirectToPage();
        }
    }
}



