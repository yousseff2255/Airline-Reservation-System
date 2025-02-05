using Airline_Reservation_System.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Text;
using iText.IO.Font.Constants;

namespace Airline_Reservation_System.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly DB db;


        public DashboardModel(DB db)
        {
            this.db = db;
        }
        public IActionResult OnPostDownloadReport()
        {
            try
            {
                // Create PDF content
                using (MemoryStream ms = new MemoryStream())
                {
                    // Initialize PDF writer
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    // Load a default font (you can customize this font or use a different one)
                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);


                    // Title section
                    document.SetFontSize(14);
                    document.Add(new Paragraph("Admin Dashboard Report")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFont(font)  // Set bold font
                        .SetFontSize(20)
                        .SetMarginBottom(20));

                    // Executive Summary section
                    document.Add(new Paragraph("Executive Summary")
                        .SetFontSize(16)
                        .SetFont(font)  // Set bold font
                        .SetMarginBottom(5));

                    document.Add(new Paragraph("This report provides a summary of the current status of the airline reservation system, covering key statistics and performance metrics.")
                        .SetFontSize(12)
                        .SetMarginBottom(20));

                    // Flight Statistics section
                    document.Add(new Paragraph("Flight Statistics")
                        .SetFontSize(16)
                        .SetFont(font)  // Set bold font
                        .SetMarginTop(20)
                        .SetMarginBottom(5));


                    document.Add(new Paragraph($"1. Total Flights: {Report["Total_flights"]}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"2. Flights Booked Today: {Report["Today's_flights"]}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"3. Scheduled Fligts: {Report["Scheduled"]}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"4. Finished Fligts: {Report["Finished"]}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"5. Delayed Fligts: {Report["Delayed"]}")
                        .SetFontSize(12));

                    // Booking Insights section
                    document.Add(new Paragraph("Booking Insights")
                        .SetFontSize(16)
                        .SetFont(font)  // Set bold font
                        .SetMarginTop(20)
                        .SetMarginBottom(5));

                    document.Add(new Paragraph($"1. Total Reservations: {Report["Total_tickets"]}")
                          .SetFontSize(12));
                    document.Add(new Paragraph($"2. Total Revenue Generated:  ${Report["Revenue"]}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"3. Passanger on board today:  {Report["Today's_Passanger"]} Passanger")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"4. Today's Revenue:  ${Report["Today's_revenue"]}")
                        .SetFontSize(12));
                    document.Add(new Paragraph($"5. Most day with flights:  {Report["Most_day"]}")
                        .SetFontSize(12));

                    // Closing the document
                    document.Close();

                    // Return the generated PDF as a downloadable file
                    return File(ms.ToArray(), "application/pdf", "Admin_Dashboard_Report.pdf");
                }
            }
            catch (Exception ex)
            {
                // Log the error and return a bad request response
                Console.WriteLine($"Error generating report: {ex.Message}");
                return BadRequest("An error occurred while generating the report.");
            }


        }


        private byte[] GenerateReport()
        {
            using (var ms = new MemoryStream())
            {
                // Initialize PdfWriter and PdfDocument
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Add content to the PDF
                document.Add(new Paragraph($"Total Flights: {Report["Total_Flights"]}"));

                // Close the document to finalize it
                document.Close();

                // Return the generated PDF content as a byte array
                return ms.ToArray();
            }
        }

        public DataTable donut { get; set; }

        public DataTable Ticket_table { get; set; }

        public DataTable Flight_table { get; set; }

        public DataTable flight_days_table { get; set; }

        public DataTable Rev { get; set; }
        public int[] status { get; set; } = new int[3];

        public int[] flight_days { get; set; } = new int[7];
        public int[] Tickets { get; set; } = new int[12];
        public int[] Flights { get; set; } = new int[12];
        public string[] Reviews { get; set; } = new string[3];

        public List<decimal> Summary { get; set; } = new List<decimal>();
        public decimal total_revenues { get; set; }

        static Dictionary<string, decimal> Report { get; set; } = new Dictionary<string, decimal>();
        public IActionResult OnGet(int year)
        {

            if (HttpContext.Session.GetString("role") != "admin")
            {
                return RedirectToPage("../Index");
            }
            Console.WriteLine(year);
            if (year == 0) { year = 2025; }


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

            Rev = db.GetReviews();
            Ticket_table = db.Admin_line_chart(year);
            Flight_table = db.Admin_line_chart1(year);
            flight_days_table = db.total_flights_day(year);
            total_revenues = db.total_revenue_year(year);
            Summary = db.summary();
            for (int i = 0; i < 12; i++)
            {


                Tickets[i] = (((int)Ticket_table.Rows[i][1]));
                Flights[i] = (((int)Flight_table.Rows[i][1]));



            }

            for (int i = 0; i < 7; i++)
            {
                flight_days[i] = (((int)flight_days_table.Rows[i][1]));

            }
            if (!Report.ContainsKey("Total_flights"))
            {
                Report.Add("Total_flights", Flights.Sum());
                Report.Add("Total_tickets", Tickets.Sum());
                Report.Add("Delayed", status[0]);
                Report.Add("Finished", status[1]);
                Report.Add("Scheduled", status[1]);
                Report.Add("Revenue", total_revenues);
                Report.Add("Most_day", flight_days.Max());
                Report.Add("Today's_flights", Summary[0]);
                Report.Add("Today's_revenue", Summary[1]);
                Report.Add("Today's_Passanger", Summary[2]);
            }
            return Page();
        }

        public JsonResult OnGetData(int year)
        {


            donut = db.Admin_pie_chart(year);
            Ticket_table = db.Admin_line_chart(year);
            Flight_table = db.Admin_line_chart1(year);
            flight_days_table = db.total_flights_day(year);
            var flight = new List<int>();
            var ticket = new List<int>();
            var day = new List<int>();
            var don = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                ticket.Add((int)Ticket_table.Rows[i][1]);
                flight.Add((int)Flight_table.Rows[i][1]);
            }
            for (int i = 0; i < 7; i++)
            {
                day.Add((int)flight_days_table.Rows[i][1]);
            }
            for (int i = 0; i < 3; i++)
            {

                don.Add((int)donut.Rows[i][0]);
            }
            int total_tickets = ticket.Sum();
            int total_flights = flight.Sum();




            return new JsonResult(new { ticket, flight, total_tickets, total_flights, day, don });
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
