namespace Airline_Reservation_System.Models
{
    public class BookingDetails
    {
        public string CustomerName { get; set; }
        public DateTime BookingDate { get; set; }
        public int SeatsCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
