using System.Data;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using Microsoft.ML;
using MyMLApp;
using Tensorflow;
using Tensorflow.Keras.Layers;
using static MyMLApp.SentimentModel;

namespace Airline_Reservation_System.Models
{
    public class DB
    {
        string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog = AirlineSystem; Integrated Security = True; Trust Server Certificate=True";
        public SqlConnection connection { get; set; }

        // Inject Prediction Engine into the constructor
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
        public DataTable AdminGetFlights(int page)
        {
            DataTable dt = new DataTable();

            string stored_proc = "get_all_flights";
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
            string Query = "select distinct model from AirCraft ";

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

        public DataTable GetAirports()
        {
            DataTable airports = new DataTable();
            string Query = "select AirPort_name , abbreviation  from AirPort ";

            SqlCommand cmd = new SqlCommand(Query, connection);

            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                airports.Load(sdr);

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

                // Filter out bad comments using ML Model
                dt = FilterBadComments(dt);
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

        private DataTable FilterBadComments(DataTable dt)
        {
            // Create a new DataTable to store filtered results
            DataTable filteredDt = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                string comment = row["comment"].ToString(); 

                // Use the generated prediction engine
                var input = new ModelInput { Col0 = comment }; 
                var result = SentimentModel.Predict(input);   

                // Add only positive comments
                if (result.PredictedLabel == 1) 
                {
                    filteredDt.ImportRow(row);
                }
            }

            return filteredDt;
        }


        #endregion


        #region Amr
        public void Book(int flight_id, int psngr_id,int num_seats , string Class)
        {
            string proc = "Book";
            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@psngr_id", psngr_id));
            cmd.Parameters.Add(new SqlParameter("@flight_id", flight_id));
            cmd.Parameters.Add(new SqlParameter("@class", Class));

            cmd.Parameters.Add(new SqlParameter("@n", num_seats));
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally { connection.Close(); }
        }


        public (List<Flight> flights, decimal price, int avaSeats) Search(Flight f, string Class, int num)
        {
            List<Flight> flights = new List<Flight>();
            string proc = "SEARCH";
            Flight f_ = new Flight();
            int ava_seats = 0;
            decimal price = 0;

            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@departure_airport", f.FromAirport));
            cmd.Parameters.Add(new SqlParameter("@arr_airport", f.ToAirport));
            cmd.Parameters.Add(new SqlParameter("@departure_time", f.LeavDate));
            cmd.Parameters.Add(new SqlParameter("@class", Class));
            cmd.Parameters.Add(new SqlParameter("@N", num));

            try
            {
                connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read()) // To handle multiple rows
                {
                    f_ = new Flight
                    {
                        FromAirport = (string)sdr["From_Airport"],
                        ToAirport = (string)sdr["AirPort_name"],
                        flight_id = (int)sdr["flight_ID"],
                        LeavDate = (DateTime)sdr["depart_datetime"],
                        ArrDate = (DateTime)sdr["arrival_datetime"]
                    };

                    // Calculate duration after properties are initialized
                    f_.Duration = (f_.ArrDate - f_.LeavDate).TotalHours;


                    ava_seats = (int)sdr["Number of avaiable seats"];
                    price = (decimal)sdr["Price"];
                    flights.Add(f_);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
            finally
            {
                connection.Close();
            }

            // Return multiple values as a tuple
            return (flights, price, ava_seats);
        }



        //public DataTable GetPastFlights(int psngr_id)
        //{
        //    DataTable PastFlights = new DataTable();
        //    string Query = $"GetPastFlights {psngr_id}";
        //    SqlCommand cmd = new SqlCommand(Query, connection);

        //    try
        //    {
        //        connection.Open();
        //        PastFlights.Load(cmd.ExecuteReader());

        //    }
        //    catch (Exception err)
        //    {
        //        Console.WriteLine(err);
        //    }
        //    finally { connection.Close(); }

        //    return PastFlights;
        //}

        public DataTable GetMyReservations(int psngr_id)
        {
            DataTable MyReservations = new DataTable();
            string proc = "GetPassReservations";

            SqlCommand cmd = new SqlCommand(proc, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@pass_id", psngr_id));

            try
            {
                connection.Open();
                MyReservations.Load(cmd.ExecuteReader());

            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally { connection.Close(); }

            return MyReservations;
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

        public int GetPassengerID(string email)
        {
            int pass_id=0;
            string query = $"SelecT pid from passenger where email = '{email}'";
            SqlCommand cmd = new SqlCommand(query,connection);
            try {
                connection.Open();
                pass_id=(int)cmd.ExecuteScalar();
            }
            catch(Exception err) {
                Console.WriteLine(err); 
            }
            finally 
            { connection.Close(); }

            return pass_id;
        }

        public void AddReview(int pass_id ,string reviewText , int flightId,int rating)
        {
            string Query = $"insert into Feedback values ({pass_id},{flightId},{rating},'{reviewText}')";
            SqlCommand cmd = new SqlCommand(Query , connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception err) {
                Console.WriteLine (err);
            }
            finally
            {
                connection.Close ();
            }
        }

        public void DeleteReservation(int flight_id , int seat , int pass_id)
        {
            string proc = "DeleteReservation";
            SqlCommand cmd = new SqlCommand(proc,connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@flight_id", flight_id));
            cmd.Parameters.Add(new SqlParameter("@seat_id", seat));
            cmd.Parameters.Add(new SqlParameter("@passenger_id", pass_id));
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception err) {
                Console.WriteLine(err);
            }
            finally
            {
                connection.Close();
            }

        }
        #endregion



        #region Essam
        public void CreateUser(string email, string password, string role)
        {
            string query = "Insert into Users (email , password, role) Values (@email , @password ,@role)";

            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("password", password);

                    cmd.Parameters.AddWithValue("role", role);
                    cmd.ExecuteNonQuery();
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
        }

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

        public DataTable Crew(int id)
        {
            DataTable dt = new DataTable();

            string query = $"select Fname,Lname,role,assigned_to.member_id from crew_member join assigned_to on crew_member.member_Id=assigned_to.member_id join flight on assigned_to.flight_id=flight.flight_ID where flight.flight_ID={id}";
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
        public DataTable getCrew(string date, string role)
        {
            DataTable dt = new DataTable();

            string query = $"select distinct c.Fname,c.Lname ,c.member_id,role\r\nfrom crew_member c left join assigned_to a on c.member_Id=a.member_id\r\nwhere (c.member_Id not in (\r\n\r\nselect member_id from flight f join assigned_to on assigned_to.flight_id=f.flight_ID\r\n WHERE '{date}'  BETWEEN DATEADD(day, -1, f.depart_datetime) AND DATEADD(day, 1, f.arrival_datetime)\r\n \r\n )\r\n\r\n or flight_id is null) and role = '{role}'";
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


        public List<decimal> summary()
        {
            List<decimal> summ = new List<decimal>();

            string querry = "select count(*) from flight\r\nwhere convert(date,depart_datetime) = CONVERT(date,getdate())";
            string querry1 = "select isnull(sum(Price),0) from Prices join flight on flight.flight_ID= Prices.flight_ID  join reservation on reservation.flight_id=flight.flight_ID  join AirCraftsSeats on AirCraftsSeats.seat_id=reservation.seat_id and AirCraftsSeats.AirCraft_id=reservation.AirCraft_ID and AirCraftsSeats.class=Prices.class\r\nwhere convert(date,flight.depart_datetime)=CONVERT(date,getdate())";
            string querry2 = "select count(*) from reservation , flight \r\nwhere reservation.flight_id=flight.flight_ID and convert(date,flight.depart_datetime)=convert(date,getdate())";
            SqlCommand cmd;
            try
            {
                connection.Open();
                cmd = new SqlCommand(querry, connection);
                summ.Add((int)cmd.ExecuteScalar());

                cmd.CommandText = querry1;


                summ.Add((decimal)cmd.ExecuteScalar());

                cmd.CommandText = querry2;

                summ.Add((int)cmd.ExecuteScalar());


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return summ;

        }

        public DataTable Flight_details(int ID)
        {
            DataTable dt = new DataTable();
            string querry = $"select flight_id,convert(date,depart_datetime),convert(time,depart_datetime),F.AirPort_name,T.AirPort_name ,AirCraft.Model,flight.status\r\nfrom flight \r\njoin AirPort F on F.AirPort_ID=flight.From_Airport_ID \r\njoin AirPort T on T.AirPort_ID=To_Airport_ID\r\njoin AirCraft on AirCraft.AirCraft_ID=flight.AirCraft_ID\r\nwhere flight.flight_ID={ID}";
            SqlCommand cmd = new SqlCommand(querry, connection);
            try
            {
                connection.Open();
                dt.Load(cmd.ExecuteReader());
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

        public void edit_flight(int id, string name, string role)
        {
            string querry = $"\twith D as(\r\n\tselect assigned_to.member_id from crew_member join assigned_to on crew_member.member_Id=assigned_to.member_id\r\n\twhere (role='{role}') and flight_Id={id}\r\n\t),\r\n\ta as(\r\n\tselect member_id from crew_member\r\n\twhere (role='{role}') and (Fname + ' ' + Lname = '{name}')\r\n\t)\r\n\t\r\n\tupdate assigned_to \r\n\tset member_id= (select member_id from a)\r\n\twhere flight_id={id} and member_id = (select member_id from D)";
            SqlCommand cmd = new SqlCommand(querry, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }



        }

        public void edit_flight_attendant(int id, string name, string role, int member_id)
        {
            string querry = $"with a as(\r\n\tselect member_id from crew_member\r\n\twhere (role='{role}') and ((Fname + ' ' + Lname) = '{name}')\r\n\t)\r\n\t\r\n\tupdate assigned_to \r\n\tset member_id= (select member_id from a)\r\n\twhere flight_id={id} and member_id = {member_id}";
            SqlCommand cmd = new SqlCommand(querry, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public DataTable seats_in_flight(int id)
        {
            DataTable dt = new DataTable();
            string proc = "seats_in_flight";
            SqlCommand cmd = new SqlCommand(proc, connection);
            try
            {
                connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }
            return dt;
        }

        public void delete_flight(int id)
        {
            string querry = $"delete flight\r\nwhere flight_ID ={id}";
            SqlCommand cmd = new SqlCommand(querry, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }
        }

        public void update_price(string price, string clas, int id)
        {
            string querry = $"UPDATE Prices\r\nSET\r\n Price = {price}\r\nWHERE \r\n Flight_Id ={id} AND\r\n  Class = '{clas}';";
            SqlCommand cmd = new SqlCommand(querry, connection);
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }
        }

        public DataTable show_price(int id)
        {
            string querry = $"select class,price from prices\r\n   where flight_id={id}\r\n   order by class";
            SqlCommand cmd = new SqlCommand(querry, connection);
            DataTable dt = new DataTable();
            try
            {
                connection.Open();
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { connection.Close(); }
            return dt;
        }
        #endregion
    }
} 
