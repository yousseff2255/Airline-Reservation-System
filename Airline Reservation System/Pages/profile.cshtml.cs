using System.Data;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages
{
    public class profileModel : PageModel
    {
        public DataTable dt { get; set; }
        public string[] queries { get; set; }
        public DB db { get; set; }
        public DataSet ds { get; set; }
        public profileModel(DB db)
        {
            queries = new string[] { };
            this.db = db;
        }
        public void OnGet()
        {   
            
            
             dt= db.GetProfileInfo();
            
        }
    }
}
