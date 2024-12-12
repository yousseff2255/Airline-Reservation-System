using System.Data;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Staff
{
    public class FlightDetailsModel : PageModel
    {
        public DB db { get; set; }
        public DataTable dt { get; set; }

        public FlightDetailsModel(DB db)
        {
     
            this.db = db;
            dt = new DataTable();
        }

        public void OnGet()
        {
            dt = db.StaffGetPassengers(1 ,23);
           
        }
    }
}
