using System.Data;
using System.Text.Json;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages
{
  
    public class SearchModel : PageModel
    {
        public DB db { get; set; }
        [BindProperty]
        public DataTable airports { get; set; }
        [BindProperty]
        public int no_psngrs { get; set; }
        [BindProperty]
        public Flight flight { get; set; }
        [BindProperty]
        public string Class { get; set; }
        public SearchModel(DB db)
        {   
            this.db = db;
            airports = new DataTable();
            flight = new Flight();
            

        }
        public void OnGet()
        {
            airports = db.GetAirports();
            
            
        }

        public IActionResult OnPost()
        {
            string jsonData = JsonSerializer.Serialize(flight);

            return RedirectToPage("/Reservation", new { jsonflight = jsonData , _class= Class , num = no_psngrs });
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