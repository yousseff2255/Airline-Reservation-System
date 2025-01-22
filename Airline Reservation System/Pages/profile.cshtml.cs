using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Security.Cryptography.Xml;
using Airline_Reservation_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Airline_Reservation_System.Pages
{
    public class profileModel : PageModel
    {
        public DataTable dt { get; set; } = new DataTable();
        public string passportInfo { get; set; }
        public string passportNo { get; set; }
        public string Country { get; set; }
        public string Issued { get; set; }
        public string Expires { get; set; }

        [BindProperty]
        public string Inputphonenumber { get; set; }

        [BindProperty]
        public string Inputaddress_info { get; set; }

        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmNewPassword { get; set; }
        public int buttonPressed { get; set; } = 0;

        public int buttonPressedmodifyaddress { get; set; } = 0;
        public DB db { get; set; }

        public profileModel(DB db)
        {

            this.db = db;
        }
        public IActionResult OnGet(string pass_email = "")
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
            {
                return RedirectToPage("Index");
            }
            else if (HttpContext.Session.GetString("role") == "staff" || HttpContext.Session.GetString("role") == "admin")
            {
                dt = db.GetProfileInfo(pass_email);


                passportInfo = (string)dt.Rows[0]["passport_info"];

                return Page();
            }
            else
            {
                //for every getProfileInfo(go get the email and pass the username to it     
                dt = db.GetProfileInfo(HttpContext.Session.GetString("email"));


                passportInfo = (string)dt.Rows[0]["passport_info"];

                return Page();
            }


    



        }
        public IActionResult OnPostModifyPassword()
        {
            //check that the old password = that from the session 
            //check that the new password and confirmedNewpassword are equal 
            if (NewPassword is null || OldPassword is null || ConfirmNewPassword is null) { return RedirectToPage(); }
            if (NewPassword == ConfirmNewPassword && OldPassword == Convert.ToString(HttpContext.Session.GetString("password")))
            {
                db.updatePassword(NewPassword, Convert.ToString(HttpContext.Session.GetString("email")));
                HttpContext.Session.SetString("password", NewPassword);
            }
            else
            {
                Console.WriteLine("Incorrect");
            }

            return RedirectToPage();
        }
        public IActionResult OnpostModifyPhoneNUmber()
        {

            // Console.WriteLine((string)dt.Rows[0]["phoneNumber"]);
            // db.updatephoneNumber(Inputphonenumber, (string)dt.Rows[0]["phoneNumber"], (string)dt.Rows[0]["email"]); 

            dt = db.GetProfileInfo(HttpContext.Session.GetString("email"));
            if (dt == null || dt.Rows.Count == 0)
            {
                Console.WriteLine("DataTable is null or empty.");
                return RedirectToPage();
            }
            buttonPressed++;
            buttonPressed = Convert.ToInt32(TempData["buttonPressed"] ?? "0") + 1;
            TempData["buttonPressed"] = buttonPressed;

            return RedirectToPage();
        }
        public IActionResult OnPostSavePhoneNumber()
        {
            dt = db.GetProfileInfo(HttpContext.Session.GetString("email"));

            db.updatephoneNumber(Inputphonenumber, Convert.ToString(dt.Rows[0]["email"]));
            return RedirectToPage();
        }
        public IActionResult OnpostModifyAddress()
        {
            //we have to modify the address in the passport also or remove the country from the passport info 
            dt = db.GetProfileInfo(HttpContext.Session.GetString("email"));
            if (dt == null || dt.Rows.Count == 0)
            {
                Console.WriteLine("DataTable is null or empty.");
                return RedirectToPage();
            }
            buttonPressedmodifyaddress++;
            buttonPressedmodifyaddress = Convert.ToInt32(TempData["buttonPressedmodifyaddress"] ?? "0") + 1;
            TempData["buttonPressedmodifyaddress"] = buttonPressedmodifyaddress;
            return RedirectToPage();
        }

        public IActionResult OnpostSaveAddress()
        {
            dt = db.GetProfileInfo(HttpContext.Session.GetString("email"));
            db.updateAddress(Inputaddress_info, (string)dt.Rows[0]["email"]);
            return RedirectToPage();
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