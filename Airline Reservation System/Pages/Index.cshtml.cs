using System.Data;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages
{
    public class IndexModel : PageModel
    {
    
        private readonly ILogger<IndexModel> _logger;
        public DataTable ReviewsTable { get; set; }
        public DB db { get; set; }



        public IndexModel(ILogger<IndexModel> logger , DB db)
        {
            _logger = logger;
            this.db = db;
            ReviewsTable = new DataTable();


        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") == "admin")
            {
                return RedirectToPage("admin/dashboard");
            }
            else if (HttpContext.Session.GetString("role") == "staff")
            {
                return RedirectToPage("staff/flights");
            }
            else
            {
                ReviewsTable = db.GetReviews();
                return Page();

            }

        }

        public IActionResult OnPostLandingB()
        {
            return RedirectToPage("/search");
        }
		public IActionResult OnPost()
		{
			return RedirectToPage("/search");
		}
        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("password");
            return RedirectToPage("signin");
        }
    }
}
