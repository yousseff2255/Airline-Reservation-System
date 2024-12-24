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
        public int page { get; set; }
        public DataTable dt { get; set; }

        public FlightsModel(DB db)
        {
            this.db = db;
            dt = new DataTable();
            FlightIDs = new List<int>();
        }
        public void OnGet(int PageNumber)
        {
            if (PageNumber == 0) PageNumber = 1;
            dt = db.StaffGetFlights(PageNumber);
            FlightIDs = db.GetFlights();
            num_flights = db.GetFlightsNumber();
            num_passengers = db.GetPassengersNumber();



        }
        public IActionResult OnPostUpdateFlightStatus(int flightNumber, string status)
        {
            db.UpdateFlightStatus(flightNumber, status);
            return new JsonResult(new { success = true });
        }

    }
}