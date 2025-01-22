using Airline_Reservation_System.Pages.Staff;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Tensorflow.Keras.Engine;
using Azure;
using static Tensorflow.TensorShapeProto.Types;

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

        public DataTable get_details { get; set; }
        public string[] getc { get; set; }

        ///////////
        public int page { get; set; }

        public DataTable dt { get; set; }

        public List<int> flights { get; set; } = new List<int>();

        public Flight f { get; set; } = new Flight();

        public int CheckedIn { get; set; }

        public int num_passengers { get; set; }

        public int capacity { get; set; }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        static List<int> member_id { get; set; } = new List<int>();

        public DataTable prices { get; set; } = new DataTable();
        public int[] seats_num { get; set; } = new int[3];
        public IActionResult OnGet(int id)
        {
            if (HttpContext.Session.GetString("role").ToLower() == "admin")
            {

                HttpContext.Session.SetInt32("id", id);
                crew = db.Crew(id);
                get_details = db.Flight_details(id);
                page = 0;

                dt = db.StaffGetPassengers(1, id);
                flights = db.GetFlights();
                CheckedIn = db.GetCheckedInNumber(id);
                capacity = db.GetAirplaneCapacity(id);
                num_passengers = db.GetPassengersNumber(id);
                f = db.GetFlightDetails(id);
                DataTable seats_num_table = new DataTable();
                seats_num_table = db.seats_in_flight(id);
                for (int i = 0; i < seats_num_table.Rows.Count; i++)
                {
                    seats_num[i] = (int)seats_num_table.Rows[i][1];
                }

                prices = db.show_price(id);

                return Page();

            }
            else
            {
				return RedirectToPage("../index");
			}

        }

        public JsonResult OnGetCrew()
        {
            id = (int)HttpContext.Session.GetInt32("id");
            get_details = db.Flight_details(id);
            getcrew = db.getCrew(get_details.Rows[0][1].ToString().Replace("12:00:00 AM", ""), "pilot");
            DataTable dt = new DataTable();
            dt = db.getCrew(get_details.Rows[0][1].ToString().Replace("12:00:00 AM", ""), "co-pilot");
            var crewNames = new List<string>();
            var conames = new List<string>();
            for (int i = 0; i < getcrew.Rows.Count; i++)
            {
                crewNames.Add(getcrew.Rows[i][0].ToString() + " " + getcrew.Rows[i][1].ToString());
                conames.Add(dt.Rows[i][0].ToString() + " " + dt.Rows[i][1].ToString());
            }

            return new JsonResult(new { crewNames, conames });
        }

        public JsonResult OnGetCabinCrew()
        {
            id = (int)HttpContext.Session.GetInt32("id");
            get_details = db.Flight_details(id);
            DataTable dt = new DataTable();
            dt = db.getCrew(get_details.Rows[0][1].ToString().Replace("12:00:00 AM", ""), "Flight Attendant");
            var Names = new List<string>();
            Console.WriteLine(dt.Rows[0][0]);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Names.Add(dt.Rows[i][0].ToString() + " " + dt.Rows[i][1].ToString());

            }

            return new JsonResult(new { Names });
        }

        public class FlightDetailsModel
        {
            public string Pilot { get; set; }
            public string CoPilot { get; set; }
        }

        public class Attendant
        {
            public string Name { get; set; }
            public string ana { get; set; }
        }

        public class Price
        {
            public string economy { get; set; }
            public string firstclass { get; set; }
            public string Business { get; set; }


        }

        [ValidateAntiForgeryToken]
        public JsonResult OnPostFun([FromBody] FlightDetailsModel data)
        {
            id = (int)HttpContext.Session.GetInt32("id");
            if (data != null)
            {
                Console.WriteLine($"Pilot: {data.Pilot}, Co-pilot: {data.CoPilot}");
                db.edit_flight(id, data.Pilot, "Pilot");
                db.edit_flight(id, data.CoPilot, "Co-Pilot");
            }
            else
            {
                Console.WriteLine("Received no data");
            }

            // Return a response back to the frontend
            return new JsonResult(new { result = "Handler executed successfully!" });
        }

        public JsonResult OnPostUpdateprice([FromBody] Price data)
        {
            id = (int)HttpContext.Session.GetInt32("id");
            if (data != null)
            {
                Console.WriteLine($"Pilot: {data.economy}, Co-pilot: {data.firstclass}");
                db.update_price(data.economy, "Economy", id);
                db.update_price(data.firstclass, "First Class", id);
                db.update_price(data.Business, "Business", id);
            }
            else
            {
                Console.WriteLine("Received no data");
            }

            // Return a response back to the frontend
            return new JsonResult(new { result = "Handler executed successfully!" });
        }

        [ValidateAntiForgeryToken]
        public JsonResult OnPostAtt([FromBody] Attendant data)
        {
            id = (int)HttpContext.Session.GetInt32("id");




            if (data != null)
            {
                crew = db.Crew(id);
                for (int i = 0; i < crew.Rows.Count; i++)
                {
                    if (crew.Rows[i][2].ToString().ToLower() == "flight attendant")
                    {
                        member_id.Add((int)crew.Rows[i][3]);

                    }
                }
                Console.WriteLine(data.Name);
                db.edit_flight_attendant(id, data.Name, "Flight Attendant", member_id[0]);
                db.edit_flight_attendant(id, data.ana, "Flight Attendant", member_id[1]);
            }
            else
            {
                Console.WriteLine("Received no data");
            }

            // Return a response back to the frontend
            return new JsonResult(new { result = "Handler executed successfully!" });
        }


        public IActionResult OnPost()
        {
            id = (int)HttpContext.Session.GetInt32("id");
            db.delete_flight(id);
            return RedirectToPage("dashboard");
        }
        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("password");
            return RedirectToPage("../signin");
        }

    }
}