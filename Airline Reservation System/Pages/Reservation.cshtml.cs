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
    public class ReservationModel : PageModel
    {
  
 
        public Flight flight { get; set; }
     

        public string Class { get; set; }

        public DataTable search { get; set; }

        private readonly DB db;

        public ReservationModel(DB database)
        {
            db = database;
            Flight flight = new Flight();
            DataTable search = new DataTable();
        }

        public void OnGet(string jsonflight , string _class)
        {
            this.Class = _class;
            flight = JsonSerializer.Deserialize<Flight>(jsonflight);
            search = db.Search(flight , this.Class);


        }

            

    }
}