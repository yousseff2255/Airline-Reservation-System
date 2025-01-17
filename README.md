# ExoSky Airlines

## Purpose

ExoSky Airlines is a user-friendly airline ticket reservation system designed to explore web development using the .NET framework, specifically Razor Pages. The project offers features like flight and user management, booking, and sentiment analysis of user reviews using ML.NET.

---

## Features

- **Admin Features**:

  - Add, update, and monitor flights and their crew.
  - Manage staff and admin accounts.

- **Staff Features**:

  - Update users' check-in status.

- **User Features**:

  - Search for flights and make reservations.
  - Cancel booked flights.
  - Submit reviews on their experience.

- **Additional Functionalities**:
  - Generate PDF reports for bookings using iText7.
  - Send booking confirmation emails using MailKit.
  - Perform sentiment analysis on user reviews using ML.NET.

---

## Prerequisites

To run the project, you will need:

- **Operating System**: Windows 10 or later.
- **.NET SDK**: Version 7.0 or higher (compatible with Razor Pages).
- **SQL Server**: SQL Server 2022 (other versions may work).
- **Visual Studio**: Version 2022 (Community, Professional, or Enterprise).

Ensure the following workloads are installed in Visual Studio:

- **ASP.NET and web development**.
- **.NET desktop development** (optional, but recommended for full compatibility).

---

## Setup Instructions

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yousseff2255/Airline-Reservation-System.git

   ```

2. **Database Setup**

   - Open SQL Server Management Studio (SSMS).
   - Restore the database schema provided in the project files.
   - Load the sample data script to populate initial records.

3. **Configure Connection String**

   - Open the `DB.cs` file in the Models folder.
   - Update the connection string to match your SQL Server instance:

     ```json
     string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog = AirlineSystem; Integrated Security = True; Trust Server Certificate=True";

     ```

4. **Install Dependencies**
   Restore NuGet packages for third-party libraries:

   ```bash
   dotnet restore
   ```

5. **Run the Application**
   - Open the project in Visual Studio.
   - Press `F5` or click **Run** to start the project.

---

## Project Structure

- **Models**: Contains entity models for the database (e.g., `Flight`, `User` , `Passenger`).
- **Pages**: Razor Pages for the frontend, including pages for users, admins, and staff.
- **Services**: Includes services for ADO.NET operations, ML.NET integration, and email handling.
- **Database**: Contains the database schema and sample data files.
- **MLModel**: Trained ML.NET model for sentiment analysis.
- **Third-Party Libraries**:
  - **iText7**: Generate PDF reports.
  - **MailKit**: Send booking confirmation emails.
  - **Newtonsoft.Json**: Handle JSON serialization.

---

## Usage Instructions

- **Admin**:

  - Login with an admin account.
  - Access the admin dashboard to manage flights, staff, and bookings.

- **Staff**:

  - View flight details and update check-in statuses.

- **User**:
  - Register or log in.
  - Search for flights, make reservations, cancel bookings, or leave reviews.

---

## Testing

- The project includes basic testing for the reservation system using built-in Razor Pages testing tools.
- Sentiment analysis can be tested by providing review samples through the user interface.

---

## License

This project is open source and available under the MIT License.
