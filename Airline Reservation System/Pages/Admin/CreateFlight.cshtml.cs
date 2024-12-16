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
        public bool b { get; set; } = true;
        public CreateFlightModel(DB db)
        {
            airports = new List<string>();
            models = new List<string>();
            flight = new Flight();
            this.db = db;
           
        }

        public void OnGet()
        {
            airports = db.GetAirports();
            models = db.GetModels();
           
        }

        public IActionResult OnPost() {
            b = db.AddFlight(flight);
            return RedirectToPage();
        }
    }
}



