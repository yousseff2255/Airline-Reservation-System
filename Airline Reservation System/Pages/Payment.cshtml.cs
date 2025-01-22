using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airline_Reservation_System.Pages
{

    public class PaymentModel : PageModel
    {
        private  DB db { get; set; }
        private EmailService emailServ { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal Booking_price { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal Total_price { get; set; }
        public decimal taxes { get; set; } = 0;
        [BindProperty]
        public int pass_id { get; set; }
        [BindProperty]
        public int fligt_id { get; set; }
        [BindProperty]
        public int num_seats { get; set; }
        [BindProperty]
        public string _class { get; set; }
        [BindProperty]
        public BookingDetails BookingInput { get; set; }

        public PaymentModel(DB db , EmailService email_ser)
        {
            this.db = db;
            emailServ = email_ser;


        }
     
        public IActionResult OnGet(int f_id , int n_seats , decimal booking_price , string Class)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
            {
                return RedirectToPage("Index");
            }

            fligt_id = f_id;
            num_seats = n_seats;
            Booking_price = booking_price;
            Total_price = taxes + Booking_price;
            _class = Class;
            return Page();
        }
        public IActionResult OnPost() {
          
            try
            {
                pass_id = db.GetPassengerID(HttpContext.Session.GetString("email"));
                db.Book(fligt_id, pass_id, num_seats, _class);
                // Your booking logic here
                // Save to database, etc.
                //BookingInput.SeatsCount = num_seats;
                //BookingInput.TotalAmount = Total_price;
                //BookingInput.BookingDate = DateTime.Now;
                //BookingInput.CustomerName = "Our Client";
                //// Send confirmation email
                //await emailServ.SendTicketConfirmationEmail(
                //    HttpContext.Session.GetString("email"),  // Replace with actual customer email
                //    BookingInput
                //);

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Handle error
                ModelState.AddModelError("", "Error processing booking.");
                return Page();
            }
            
        }
        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("password");
            return RedirectToPage("index");
        }


    }
}
