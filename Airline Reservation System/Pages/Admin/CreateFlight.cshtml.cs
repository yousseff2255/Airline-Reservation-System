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

        public Flight flight { get; set; }
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

        public void OnPost() {
            
        }
    }
}



