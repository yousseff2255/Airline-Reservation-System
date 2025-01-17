using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using static TorchSharp.torch.distributions.constraints;

namespace Airline_Reservation_System.Pages
{
   
    public class ReservationModel : PageModel
    {
        [BindProperty]
        public int sel_flight_id { get; set; }

      
        public Flight flight { get; set; }
        [BindProperty]
        public decimal price { get; set; }
        [BindProperty]
        public int num_available_seats { get; set; }
        [BindProperty(SupportsGet = true)]
        public int num_pass { get; set; }
        [BindProperty(SupportsGet =true)]
        public string Class { get; set; }

        [BindProperty]
        public List<Flight> flights { get; set; }
        private readonly DB db;

        public ReservationModel(DB database)
        {
            db = database;
            Flight flight = new Flight();
         
            flights = new List<Flight>();
        }


        public IActionResult OnGet(string jsonflight , string _class , int num)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
            {
                return RedirectToPage("index");
            }
            num_pass = num;
            Console.WriteLine(num.ToString());
            this.Class = _class;
            flight = JsonSerializer.Deserialize<Flight>(jsonflight);
           (flights, price , num_available_seats) = db.Search(flight,Class , num );
            price = price * num_pass;

            return Page();
           

        }
        public IActionResult OnPost(int f_id)
        {
            sel_flight_id = f_id;
            return RedirectToPage("Payment" , new {f_id = sel_flight_id , n_seats = num_pass , booking_price  = price , Class = this.Class});
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