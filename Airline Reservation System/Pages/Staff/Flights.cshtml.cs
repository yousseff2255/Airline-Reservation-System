using System.Data;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Staff
{
    public class FlightsModel : PageModel
    {
        public int num_flights { get; set; }
        public int num_passengers { get; set; }
        public DB db { get; set; }
        public List<int> FlightIDs { get; set; }
        public int page { get; set; } = 1;
        public DataTable dt { get; set; }

        public FlightsModel(DB db)
        {
            this.db = db;
            dt = new DataTable();
            FlightIDs = new List<int>();
        }
        public void OnGet()
        {
            dt = db.StaffGetFlights(page);
            FlightIDs = db.GetFlights();
            num_flights = db.GetFlightsNumber();
            num_passengers = db.GetPassengersNumber();



        }
    }
}