using System.Data;
using Airline_Reservation_System.Models;
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

     
        public FlightDetailsModel(DB db)
        {
     
            this.db = db;
            dt = new DataTable();
            flights = new List<int>();
            f = new Flight();
            actual_flight_num= S_flight_num;
        }

        public void OnGet()
        {

            dt = db.StaffGetPassengers(1 , actual_flight_num);
            flights = db.GetFlights();
            CheckedIn = db.GetCheckedInNumber(actual_flight_num);
            capacity=db.GetAirplaneCapacity(actual_flight_num);
            num_passengers = db.GetPassengersNumber(actual_flight_num);
            f=db.GetFlightDetails(actual_flight_num);
           
        }

        public IActionResult OnPost(int selectedFlight)
        {
            S_flight_num = selectedFlight;

            
            return RedirectToPage();
        }

        public IActionResult OnPostCheckIn(string id)
        {
            


            return RedirectToPage();
        }

    }
}
