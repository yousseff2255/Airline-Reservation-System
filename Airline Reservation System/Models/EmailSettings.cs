﻿namespace Airline_Reservation_System.Models;



public class EmailSettings
{

    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
}
