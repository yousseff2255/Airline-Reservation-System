using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Airline_Reservation_System.Pages
{
    [BindProperties(SupportsGet = true)]
    public class Index1Model : PageModel
    {
        private readonly DB db;
        static int passenger_id { get; set; }
        public Index1Model(DB database)
        {
            db = database;
            DataTable MyReservations = new DataTable();
        }
        public DataTable MyReservations { get; set; }
        public void OnGet()
        {
            passenger_id = db.GetPassengerID(HttpContext.Session.GetString("email"));
            //passenger_id = db.GetPassengerID("sabramov1@jigsy.com");
            MyReservations = db.GetMyReservations(passenger_id);
        }

        public IActionResult OnPostReview(int flightId, int rating, string reviewText)
        {
           
            db.AddReview(passenger_id , reviewText , flightId,  rating);
            return RedirectToPage();
        }

        public IActionResult OnPostCancel(int FlightId1 , int seat  )
        {
            db.DeleteReservation(FlightId1, seat, passenger_id);
            Console.WriteLine("Flight Number : " , FlightId1.ToString());
            Console.WriteLine("Seat Number : ", seat.ToString());
            return RedirectToPage();
        }
        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("password");
            return RedirectToPage("index");
        }
    }
}