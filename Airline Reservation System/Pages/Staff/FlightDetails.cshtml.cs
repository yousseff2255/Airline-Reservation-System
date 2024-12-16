using System.Data;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Staff
{
    public class FlightDetailsModel : PageModel
    {
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
        }

        public void OnGet()
        {
            dt = db.StaffGetPassengers(1 ,23);
            flights = db.GetFlights();
            CheckedIn = db.GetCheckedInNumber(23);
            capacity=db.GetAirplaneCapacity(5);
            num_passengers = db.GetPassengersNumber(23);
            f=db.GetFlightDetails(23);
           
        }
    }
}
