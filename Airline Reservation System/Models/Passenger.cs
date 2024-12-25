using System.ComponentModel.DataAnnotations;

namespace Airline_Reservation_System.Models
{
    public class Passenger : User
    {
        [Required(ErrorMessage = "First name is required.")]
        [MinLength(5, ErrorMessage = "first name Must conatin 5 characters at minimum.")]
        public string fname { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(5, ErrorMessage = "Last name Must conatin 5 characters at minimum.")]
        public string lname { get; set; }
        [Required(ErrorMessage = "Nationality is required.")]
        public string nationality { get; set; }
        [Required(ErrorMessage = "National ID is required.")]
        public int n_id { get; set; }
        [Required(ErrorMessage = "Passport Info is required.")]
        public string passport_info { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Birthdate is required.")]
        public DateOnly Bdate { get; set; }
     
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8,ErrorMessage = "Password Must conatin 8 characters at minimum.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(password), ErrorMessage = "Passwords do not match.")]
        public string conf_password { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public char gender { get; set; }
        
        [Required(ErrorMessage = "Address is required.")]
        public string address { get; set; }

        public Passenger() {
            role = "Passenger";
        }
    }
}
