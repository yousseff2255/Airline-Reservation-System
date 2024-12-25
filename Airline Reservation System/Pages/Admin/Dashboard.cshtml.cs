using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Airline_Reservation_System.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly DB db;
        

        public DashboardModel(DB db) 
        {
            this.db = db;
        }

        public DataTable donut { get; set; }

        public DataTable Ticket_table { get; set; }

        public DataTable Flight_table { get; set; }

        public DataTable flight_days_table { get; set; }

        public int[] status { get; set; } = new int[3];

        public int[] flight_days { get; set; } = new int[7];
        public int[] Tickets { get; set; } = new int[12];
        public int[] Flights { get; set; } = new int[12];

        public decimal total_revenues { get; set; }

        public IActionResult OnGet(int year)
        {
            
            if (HttpContext.Session.GetString("role") == "passanger" || string.IsNullOrEmpty(HttpContext.Session.GetString("role")))
            {
                return RedirectToPage("/Index");
            }
            Console.WriteLine(year);
           if (year == 0) { year = 2024; }

             
            donut = db.Admin_pie_chart(year);

            
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    status[i] = (((int)donut.Rows[i][0]));
                }
                catch
                {
                    status[i] = 0;
                }
            }
            
            
            Ticket_table= db.Admin_line_chart(year);
            Flight_table = db.Admin_line_chart1(year);
            flight_days_table = db.total_flights_day(year);
            total_revenues=db.total_revenue_year(year);
           
            for (int i = 0; i < 12; i++)
            {


                Tickets[i] = (((int)Ticket_table.Rows[i][1]));
                Flights[i] = (((int)Flight_table.Rows[i][1]));
                


            }

            for (int i = 0; i < 7; i++)
            {
                flight_days[i]= (((int)flight_days_table.Rows[i][1]));

            }

            return Page();
        }

        public JsonResult OnGetData(int year)
        {


            
            Ticket_table = db.Admin_line_chart(year);
            Flight_table = db.Admin_line_chart1(year);
            flight_days_table = db.total_flights_day(year);
            var flight = new List<int>();
            var ticket = new List<int>();
            var day = new List<int>();

            for (int i = 0; i < 12; i++)
            {
                ticket.Add((int)Ticket_table.Rows[i][1]);
                flight.Add((int)Flight_table.Rows[i][1]);
            }
            for(int i = 0;i < 7; i++)
            {
                day.Add((int)flight_days_table.Rows[i][1]);
            }
            int total = ticket.Sum();

            


            return new JsonResult(new { ticket,flight,total,day });
        }
    }

}
