namespace Airline_Reservation_System.Models
{
    public class Flight
    {
        public string ToAirport { get; set; }
        public string FromAirport { get; set; }
        public string AircraftModel { get; set; }
        public string plane_capacity { get; set; }
        public string Status { get; set; }
        public DateTime ArrDate { get; set; }
        public DateTime LeavDate { get; set; }
        public int gate { get; set; }
        public int num_passengers { get; set; }
        public string Co_Pilot { get; set; }
        public string Pilot { get; set; }
        public List<string> flight_att { get; set; }
        public int flight_id { get; set; }
        public double Duration { get; set; }

      



    }
}
