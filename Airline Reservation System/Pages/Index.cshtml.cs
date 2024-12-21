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

        public void OnGet()
        {
            ReviewsTable = db.GetReviews();

        }

        public IActionResult OnPostLandingB()
        {
            return RedirectToPage("/search");
        }
    }
}
