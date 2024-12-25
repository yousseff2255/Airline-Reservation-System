using Airline_Reservation_System.Pages.Staff;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Airline_Reservation_System.Pages.Admin
{
    public class FlightDetails1Model : PageModel
    {
        private readonly DB db;


        public FlightDetails1Model(DB db)
        {
            this.db = db;
        }

        public DataTable crew { get; set; }
        public DataTable getcrew { get; set; }

        public string[] getc { get; set; }
        public void OnGet()
        {
            crew = db.Crew();
            
        }

        public JsonResult OnGetCrew()
        {
            
            getcrew = db.getCrew();
            var crewNames = new List<string>();

            for (int i = 0; i < getcrew.Rows.Count; i++)
            {
                crewNames.Add(getcrew.Rows[i][0].ToString() + " " + getcrew.Rows[i][1].ToString());
            }

            return new JsonResult(new { crewNames });
        }

        public class FlightDetailsModel
        {
            public string Name { get; set; }
            public string Role { get; set; }
        }

        
        
            [ValidateAntiForgeryToken]
            public JsonResult OnPostFun([FromBody] FlightDetailsModel data)
            {
                if (data != null)
                {
                    Console.WriteLine($"Name: {data.Name}, Role: {data.Role}");
                }
                else
                {
                    Console.WriteLine("Received no data");
                }

                // Return a response back to the frontend
                return new JsonResult(new { result = "Handler executed successfully!" });
            }
        



    }
}
