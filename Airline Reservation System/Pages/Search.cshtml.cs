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
        public List<string> airports { get; set; }
        [BindProperty]
        public int no_psngrs { get; set; }
        [BindProperty]
        public Flight flight { get; set; }
        [BindProperty]
        public string Class { get; set; }
        public SearchModel(DB db)
        {   
            this.db = db;
            airports = new List<string>();
            flight = new Flight();


        }
        public void OnGet()
        {
            airports = db.GetAirports();
            
            
        }

        public IActionResult OnPost()
        {
            string jsonData = JsonSerializer.Serialize(flight);

            return RedirectToPage("/Reservation", new { jsonflight = jsonData , _class= Class });
        }
    }
}