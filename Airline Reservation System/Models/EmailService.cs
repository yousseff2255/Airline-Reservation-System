using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;


namespace Airline_Reservation_System.Models
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;  // Note the .Value here
        }

        public async Task SendTicketConfirmationEmail(string customerEmail, BookingDetails booking)
        {
            // Create a new email message
            var email = new MimeMessage();

            // Set the sender
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));

            // Set the recipient
            email.To.Add(MailboxAddress.Parse(customerEmail));

            // Set the subject
            email.Subject = "Your Ticket Booking Confirmation";

            // Create the email body
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <h2>Booking Confirmation</h2>
                <p>Dear {booking.CustomerName},</p>
                <p>Thank you for your booking. Here are your ticket details:</p>
                <ul>
                  
             
                    <li>Date: {booking.BookingDate:dd/MM/yyyy HH:mm}</li>
                    <li>Number of Tickets: {booking.SeatsCount}</li>
                    <li>Total Amount: {booking.TotalAmount:C}</li>
                </ul>
                <p>Please keep this email for your records.</p>"
            };

            email.Body = builder.ToMessageBody();

            // Send the email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
