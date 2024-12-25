using System.Data;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using Tensorflow.Keras.Layers;

namespace Airline_Reservation_System.Models
{
    public class DB
    {
         string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog = Airline System; Integrated Security = True; Trust Server Certificate=True";
        public SqlConnection connection { get; set; }

        public DB()
        {
            connection = new SqlConnection(connectionString);
        }

        #region Youssef

        public void CheckIn(string email, int flight_id)
        {

            string proc = "CheckIn";
            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@email", email));
            cmd.Parameters.Add(new SqlParameter("@flight", flight_id));
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally
            {
                connection.Close();
            }
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
            catch (Exception e)
            {
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

        public DataTable StaffGetPassengers(int page, int flight_id)
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
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    f.LeavDate = DateTime.Parse(reader["depart_datetime"].ToString());

                    f.ToAirport = reader["Airport_name"].ToString();
                    f.gate = (int)reader["gate"];
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
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    Console.WriteLine("No available aircraft found for the given model.");
                    b = false;
                }
            }
            finally
            {
                connection.Close();


            }
            return b;


        }



        public void UpdateFlightStatus(int flight_id, string NewStatus)
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
            finally
            {
                connection.Close();
            }
        }

        public DataTable GetReviews()
        {
            DataTable dt = new DataTable();
            string proc = "GetReviews";
            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
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
            finally
            {
                connection.Close();
            }
            return dt;
        }

        #endregion


        #region Amr
        public void Book(int flight_id, int psngr_id, DateTime t, int AC_id, int seat_id)
        {
            string query = $"exec Book\r\n  @psngr_id = {psngr_id},\r\n  @seat_id = {seat_id},\r\n  @AC_id = {AC_id}, \r\n  @t = {t},\r\n  @flight_id = {flight_id};";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                cmd.ExecuteReader();

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally { connection.Close(); }
        }


        public DataTable Search(Flight f, string Class)
        {
            DataTable search = new DataTable();
            string proc = "SEARCH";

            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@departure_airport", f.FromAirport));
            cmd.Parameters.Add(new SqlParameter("@arr_airport", f.ToAirport));
            cmd.Parameters.Add(new SqlParameter("@departure_time", f.LeavDate));
            cmd.Parameters.Add(new SqlParameter("@class", Class));
            cmd.Parameters.Add(new SqlParameter("@N", f.num_passengers));



            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                search.Load(sdr);

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally { connection.Close(); }

            return search;
        }


        public DataTable GetPastFlights(int psngr_id)
        {
            DataTable PastFlights = new DataTable();
            string Query = $"GetPastFlights {psngr_id}";
            SqlCommand cmd = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                PastFlights.Load(cmd.ExecuteReader());

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally { connection.Close(); }

            return PastFlights;
        }


        public DataTable ReadTable(string TableName)
        {
            DataTable dataTable = new DataTable();
            string Query = $"select * from {TableName}";
            SqlCommand cmd = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                dataTable.Load(cmd.ExecuteReader());

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally { connection.Close(); }


            return dataTable;
        }


        public void AddPassenger(Passenger p)
        {
            string query = $"insert into passenger values('{p.fname}' , '{p.lname}' , '{p.Bdate}' , {p.n_id} , '{p.gender}' , '{p.email}' , '{p.passport_info}'  , {p.phone} ,'{p.address}' , '{p.nationality}' , {p.password} )";
            SqlCommand cmd = new SqlCommand (query, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally {
                connection.Close();
             }
        }
        #endregion



        #region Essam


        public int CheckSignIN(string username, string password)
        {
            int successful = 0;

            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("ValidateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("input_username", username);
                    cmd.Parameters.AddWithValue("input_password", password);

                    SqlParameter output = new SqlParameter("Found", SqlDbType.Bit) { Direction = ParameterDirection.Output };


                    cmd.Parameters.Add(output);
                    cmd.ExecuteNonQuery();
                    successful = Convert.ToInt32(output.Value);

                    Console.WriteLine(successful);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return successful;
        }


        public DataTable GetProfileInfo(string username)
        {
            DataTable dt = new DataTable();
            string proc = "Passengerprofile";
            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Username", username));

            try
            {
                connection.Open(); // Ensure the connection is open


                SqlDataReader sdr = cmd.ExecuteReader();
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


        public string gettheUserRole(string username, string password)
        {
            string l = "";
            string query = $"select role from users where email = '{username}' and password = '{password}' ";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                


                    l= (string)cmd.ExecuteScalar();

             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return l;
        }


        public void updatephoneNumber(string newPhoneNumber, string email)
        {
            Thread.Sleep(5000); // Temporary delay for debugging
            string query = "UPDATE passenger SET phoneNumber = @newPhoneNumber WHERE  email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@newPhoneNumber", newPhoneNumber ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public void updatePassword(string newPassword, string Email)
        {


            string query = "Update Users set password = @newPassword where email = @Email";


            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@newPassword", newPassword);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.ExecuteNonQuery();
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


        }

        public void updateAddress(string newaddress, string email)
        {

            Thread.Sleep(5000);
            string query = "Update passenger set address_info = @newaddress where  email = @Email";


            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@newaddress", newaddress ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
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


        }









        #endregion

        #region Adam

        public DataTable Admin_pie_chart(int y)
        {
            DataTable dt = new DataTable();

            string query = "Admin_pie_chart";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Dyear", y));
                dt.Load(cmd.ExecuteReader());

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;


        }
        public DataTable Admin_line_chart(int year)
        {
            DataTable ticketTable = new DataTable();

            string query = "Admin_line_chart";


            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Dyear", year));

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())


                        ticketTable.Load(reader);

                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");

                }
                finally { connection.Close(); }
            }

            return ticketTable;
        }


        public DataTable Admin_line_chart1(int year)
        {
            DataTable Flighttable = new DataTable();

            string query = "Admin_line_chart1";


            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Dyear", year));

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())


                        Flighttable.Load(reader);

                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");

                }
                finally { connection.Close(); }
            }

            return Flighttable;
        }

        public DataTable Crew()
        {
            DataTable dt = new DataTable();

            string query = "select Fname,Lname,role from crew_member join assigned_to on crew_member.member_Id=assigned_to.member_id join flight on assigned_to.flight_id=flight.flight_ID where flight.flight_ID=30";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                dt.Load(cmd.ExecuteReader());

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;


        }
        public DataTable getCrew()
        {
            DataTable dt = new DataTable();

            string query = "select Fname,Lname,role from crew_member join assigned_to on crew_member.member_Id=assigned_to.member_id join flight on assigned_to.flight_id=flight.flight_ID";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                dt.Load(cmd.ExecuteReader());

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;


        }
        public DataTable total_flights_day(int y)
        {
            DataTable dt = new DataTable();

            string query = "total_flights_day";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dyear", y));
                dt.Load(cmd.ExecuteReader());

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;


        }

        public decimal total_revenue_year(int y)
        {
            decimal l = 0;

            string query = "total_revenue_year";
            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dyear", y));
                l = (decimal)cmd.ExecuteScalar();

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return l;


        } 
        #endregion
    }
} 
