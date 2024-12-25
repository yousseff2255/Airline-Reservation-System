using System.Data;
using Airline_Reservation_System.Models;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Staff
{
    public class FlightDetailsModel : PageModel
    {

        static int S_flight_num { get; set; } = 23;
        public int actual_flight_num { get; set; }
        public Flight f { get; set; }
        public DB db { get; set; }
        public int CheckedIn { get; set; }
        public int num_passengers { get; set; }
        public int capacity { get; set; }
        public List<int> flights { get; set; }
        public DataTable dt { get; set; }
        [BindProperty(SupportsGet = true)]
        public int page { get; set; }
  


        public FlightDetailsModel(DB db)
        {
     
            this.db = db;
            dt = new DataTable();
            flights = new List<int>();
            f = new Flight();
            actual_flight_num= S_flight_num;
        }

        public IActionResult OnGet(int PageNumber)
        {
            if (HttpContext.Session.GetString("role") == "passanger" || string.IsNullOrEmpty(HttpContext.Session.GetString("role")))
            {
                return RedirectToPage("/Index");
            }
            page = PageNumber;
            if (page==0) page = 1;
            
            dt = db.StaffGetPassengers(page, actual_flight_num);
            flights = db.GetFlights();
            CheckedIn = db.GetCheckedInNumber(actual_flight_num);
            capacity=db.GetAirplaneCapacity(actual_flight_num);
            num_passengers = db.GetPassengersNumber(actual_flight_num);
            f=db.GetFlightDetails(actual_flight_num);
            return Page();
           
        }

        public IActionResult OnPost(int selectedFlight)
        {
            S_flight_num = selectedFlight;

            
            return RedirectToPage();
        }

        public IActionResult OnPostCheckIn(string email)
        {

            db.CheckIn(email, actual_flight_num);

            return RedirectToPage();
        }

    }
}
