using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages
{
   
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public char gender { get; set; }
        public DB db { get; set; }
        [BindProperty]
        public Passenger p { get; set; }
        public SignUpModel(DB db)
        {
            this.db = db;
            p = new Passenger();
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost() {
            
            if (ModelState.IsValid)
            {
                p.gender = this.gender;
                db.AddPassenger(p);
                return RedirectToPage("Index");
            }
               
            else return Page();

        }
    }
}
