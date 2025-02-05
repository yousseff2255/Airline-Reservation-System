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
        public IActionResult OnGet(int PageNumber)
        {
            if (HttpContext.Session.GetString("role").ToLower() == "staff")
            {
                page = PageNumber;
                if (PageNumber == 0) PageNumber = 1;
                dt = db.StaffGetFlights(PageNumber);
                FlightIDs = db.GetFlights();
                num_flights = db.GetFlightsNumber();
                num_passengers = db.GetPassengersNumber();

                return Page();

            }
            else
            {
                return RedirectToPage("../index");
            }
      

        }
        public IActionResult OnPostUpdateFlightStatus(int flightNumber, string status)
        {
            db.UpdateFlightStatus(flightNumber, status);
            return new JsonResult(new { success = true });
        }

        public  IActionResult OnPostViewFlight(string flightId)
        {
       

            return RedirectToPage("../Admin/flightDetails1", new { id = flightId });

          
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