using System.Data;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;

namespace Airline_Reservation_System.Models
{
    public class DB
    {
        string ConnectionString = "Data Source = LOCALHOST\\SQLEXPRESS; Initial Catalog = Airline System ; Integrated Security = true ; trust server certificate = true";
        public SqlConnection connection { get; set; }

        public DB()
        {
            connection = new SqlConnection(ConnectionString);
        }

        public DataTable StaffGetFlights(int page)
        {
            DataTable dt = new DataTable();

            string stored_proc = "GetTodayFlights";
            SqlCommand cmd = new SqlCommand(stored_proc, connection);

            try
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@page", page));
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public List<int> GetFlights()
        {
            List<int> flights = new List<int>();
            string query = "SELECT flight_id FROM flight"; // Query to fetch the desired column
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    // Add the value from the column to the list
                    flights.Add(sdr.GetInt32(0)); // 0 is the index of the column
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }

            return flights;
        }

        public int GetFlightsNumber()
        {
            int n = 0;

            string Query = "SELECT COUNT(*) FROM FLIGHTS";
            SqlCommand cmd = new SqlCommand(Query, connection);
            try
            {

                connection.Open();
                n = (int)cmd.ExecuteScalar();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return n;
        }

        public int GetPassengersNumber()
        {
            int n = 0;

            string Query = "select count(distinct pid ) as [Number of passengers] from passenger as p join reservation as r on (p.PId = r.passenger_id) ";
            SqlCommand cmd = new SqlCommand(Query, connection);
            try
            {

                connection.Open();
                n = (int)cmd.ExecuteScalar();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return n;
        }


        public List<string> GetModels()
        {
            List<string> models = new List<string>();
            string Query = "select model from AirCraft ";

            SqlCommand cmd = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                  
                    models.Add(sdr.GetString(0)); 
                }

            }
            catch (Exception e) {
            Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }



            return models;
        }

        public List<string> GetAirports()
        {
            List<string> airports = new List<string>();
            string Query = "select AirPort_name from AirPort ";

            SqlCommand cmd = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {

                    airports.Add(sdr.GetString(0));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }



            return airports;
        }

        public DataTable StaffGetPassengers(int page , int flight_id)
        {
            DataTable dt = new DataTable();

            string stored_proc = "GetFlightDetails";
            SqlCommand cmd = new SqlCommand(stored_proc, connection);

            try
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@page", page));
                cmd.Parameters.Add(new SqlParameter("@flight_id", flight_id));
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public int GetPassengersNumber(int flight_id)
        {
            int n = 0;

            string Query = $" select count(passenger_id) from reservation  where flight_id = {flight_id}";
            SqlCommand cmd = new SqlCommand(Query, connection);
            try
            {

                connection.Open();
                n = (int)cmd.ExecuteScalar();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return n;
        }

        public int GetCheckedInNumber(int flight_id)
        {
            int n = 0;

            string Query = " select dbo.GetNumChecked (@flight_id)";
            SqlCommand cmd = new SqlCommand(Query, connection);

            cmd.Parameters.AddWithValue("@flight_id", flight_id);
       
            try
            {

                connection.Open();
                n = (int)cmd.ExecuteScalar();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return n;
        }

        public int GetAirplaneCapacity(int flight_id)
        {
            int n = 0;

            string Query = "select EnoSeats + BnoSeats + FnoSeats as [Total number of seats] from AirCraft as a join flight as f on (a.aircraft_id = f.aircraft_id) where flight_id = @id ";
            SqlCommand cmd = new SqlCommand(Query, connection);
            cmd.Parameters.AddWithValue("@id", flight_id);
            try
            {

                connection.Open();
                n = (int)cmd.ExecuteScalar();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
            return n;
        }
        public Flight GetFlightDetails(int flight_id)
        {
            Flight f = new Flight();
            string proc = "GetFlightBasicInfo";

            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@id", flight_id));
            try { 
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    f.LeavDate = DateTime.Parse(reader["depart_datetime"].ToString());

                    f.ToAirport = reader["Airport_name"].ToString();
                    f.gate = (int)reader["gate"];
                }


            }
            catch(Exception e) { 
                Console.WriteLine(e.Message);
            }
            finally {
                connection.Close();
            }

            return f;

        }

        public bool AddFlight(Flight f)
        {
            bool b = true;
            string proc = "AddFlight";
            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                cmd.Parameters.Add(new SqlParameter("@DeptDate", f.LeavDate));
                cmd.Parameters.Add(new SqlParameter("@ArrDate", f.ArrDate));
                cmd.Parameters.Add(new SqlParameter("@status", f.Status));
                cmd.Parameters.Add(new SqlParameter("@ToAirport", f.ToAirport));
                cmd.Parameters.Add(new SqlParameter("@FromAirport", f.FromAirport));
                cmd.Parameters.Add(new SqlParameter("@AircraftModel", f.AircraftModel));
                cmd.Parameters.Add(new SqlParameter("@gate", f.gate));
                cmd.ExecuteNonQuery(); 
                


            }
            catch (SqlException ex )
            {
                if (ex.Number == 50000) 
                {
                    Console.WriteLine("No available aircraft found for the given model.");
                    b= false;
                }
            }
            finally
            {
                connection.Close();
             

            }
            return b;


        }

        public DataTable GetProfileInfo()
        {
            DataTable dt = new DataTable();
            string proc = "Passengerprofile";
            SqlCommand cmd = new SqlCommand(proc , connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Username", "ahmed.ali@email.com"));

            try
            {
                connection.Open(); // Ensure the connection is open

                
                SqlDataReader sdr =  cmd.ExecuteReader();
                dt.Load(sdr);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message); // Debug any exceptions
            }
            finally
            {
                connection.Close(); // Always close the connection
            }

            return dt;
        }

        public void UpdateFlightStatus(int flight_id , string NewStatus)
        {
            string query = $"UPDATE flight set status = '{NewStatus}' where flight_id =  {flight_id} ";
            SqlCommand cmd = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            finally { 
                connection.Close();
            }
        }

        public DataTable GetReviews() {
            DataTable dt = new DataTable();
            string proc = "GetReviews";
            SqlCommand cmd = new SqlCommand(proc , connection);
            cmd.CommandType  = CommandType.StoredProcedure;
            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);


            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            finally {
                connection.Close();
            }
            return dt;
        }





    }
}