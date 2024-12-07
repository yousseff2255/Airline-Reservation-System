using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages.Admin
{
    public class DashboardModel : PageModel
    {
		public int[] SalesData { get; set; } = { 60, 60, 60, 60, 70, 60, 60, 60, 60, 60, 60, 60 };
		public void OnGet()
        {
        }
    }
}
