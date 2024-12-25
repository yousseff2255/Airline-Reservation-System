using System.ComponentModel.DataAnnotations;

namespace Airline_Reservation_System.Models
{
    public class User
    {
        [Required(ErrorMessage = "Email is required.")]
        public string email { get; set; }
        public string role { get; set; } = "User";
    }
}
