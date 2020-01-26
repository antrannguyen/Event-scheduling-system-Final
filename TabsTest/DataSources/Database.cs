using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System;

namespace DataSources
{

    public class Connection
    {
        private static Connection instance = null;
        private static readonly object padlock = new object();

        private const string DATASOURCE = "54.210.98.212";
        private const int PORT = 27516;
        private const string USER_NAME = "orange";
        private const string PASSWORD = "FEgglu6.3Eby2Ga5BA8dF.n7DvDvTSmu1koB";

        private static MySqlConnection connection = new MySqlConnection($"datasource={DATASOURCE};port={PORT};username={USER_NAME};password={PASSWORD}");

        private Connection()
        {
            // Empty
        }

        public static Connection Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Connection();
                    }
                    return instance;
                }
            }
        }

        public static DataTable GetEventType()
        {
            DataTable results = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT * FROM eventsdatabase.eventtypes;", connection);
            adapter.Fill(results);
            return results;
        }

        public static DataTable InsertAccounts()
        {
            DataTable results = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT * FROM eventsdatabase.users;", connection);
            adapter.Fill(results);
            return results;
        }


        public static string[] GetAllDataForUser(string userName)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM eventsdatabase.users WHERE name=?username  ;";
            cmd.Parameters.AddWithValue("?username", userName);

            MySqlDataReader reader = cmd.ExecuteReader();

            string[] result = new string[6];

            while (reader.Read())
            {
                result[0] = reader["userid"].ToString();
                result[1] = reader["name"].ToString();
                result[2] = reader["phonenumber"].ToString();
                result[3] = reader["roleid"].ToString();
                result[4] = reader["password"].ToString();
                result[5] = reader["enabled"].ToString();
            }

            connection.Close();
            return result;
        }


        public static int GetUserRoleID(string userName, string password)
        {

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT roleid FROM eventsdatabase.users WHERE name=?username  AND password=?password AND enabled > 0;";
            cmd.Parameters.AddWithValue("?username", userName);
            cmd.Parameters.AddWithValue("?password", password);


            var reader = cmd.ExecuteScalar();

            int result = 404404404;

            if (reader != null)
            {
                int.TryParse(reader.ToString(), out result);
            }

            connection.Close();

            return result;

            /*
            if (reader.FieldCount.ToString())
            {
                return "HAS";
            }

            else
            {
                return "DOESNTHAVE";
            }
            */

            /*
            while (reader.Read())
            {
                return reader.GetValue(5).ToString();

                // return reader.GetString("roleid").ToString();
            }

            return reader["roleid"].ToString();
            */

            /*
            string roleid = "403" + userName + password;

            if (reader.HasRows)
            {
                roleid = "200";
            }

            while (reader.Read())
            {
                roleid = reader[0].ToString();
            }
            return roleid;*/
        }



        public static string GetUser(string userName, string password)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM eventsdatabase.users WHERE  name='?username' AND password ='?password'     ;";
            cmd.Parameters.AddWithValue("?username", userName);
            cmd.Parameters.AddWithValue("?password", password);

            try
            {
                connection.Open();
            }
            catch
            {
                // MessageBox.Show("Error");
            }

            MySqlDataReader reader = cmd.ExecuteReader();
            string user = "";

            while (reader.Read())
            {
                user = reader["roleid"].ToString();
            }
            connection.Close();
            return user;
        }
        public static DataTable[] GetAllKeywords(int i)
        {
            DataTable results1 = new DataTable();
            DataTable results2 = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT keyword FROM eventsdatabase.keywordsspecialist WHERE orgid = " + i + ";", connection);
            adapter.Fill(results1);
            adapter = new MySqlDataAdapter("SELECT keyword FROM eventsdatabase.keywordstarget WHERE orgid = " + i + ";", connection);
            adapter.Fill(results2);
            DataTable[] arr = new DataTable[2] { results1, results2 };
            return arr;
        }


        public static DataTable GetAllEvents()
        {
            DataTable results = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT * FROM eventsdatabase.events;", connection);
            adapter.Fill(results);
            return results;
        }
        public static DataTable GetOrg()
        {
            DataTable results = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT name, userid FROM eventsdatabase.users WHERE roleid = 2;", connection);
            adapter.Fill(results);
            return results;
        }
        public static DataTable GetET()
        {
            DataTable results = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT name, eventtypeid FROM eventsdatabase.eventtypes;", connection);
            adapter.Fill(results);
            return results;
        }
        public static DataTable GetLocationEvent()
        {
            DataTable results = new DataTable();

            var adapter = new MySqlDataAdapter("SELECT info, locationid FROM eventsdatabase.locations;", connection);
            adapter.Fill(results);
            return results;
        }

        public static bool InsertEvent(int eType, int eOrg, string eName, string eDate, int eLocation, string eParticipants, string eDura)
        {
            connection.Open();

            MySqlCommand insertCommand = connection.CreateCommand();
            MySqlCommand insertCommand1 = connection.CreateCommand();


            insertCommand.CommandText = "INSERT INTO eventsdatabase.events (eventtype, organizerid, name, date, locationid, participants,duration, enabled) VALUES (@type, @org, @name, @date, @loca, @particapants, @dura, 1);";
            insertCommand.Parameters.AddWithValue("@type", eType);
            insertCommand.Parameters.AddWithValue("@org", eOrg);
            insertCommand.Parameters.AddWithValue("@name", eName);
            insertCommand.Parameters.AddWithValue("@date", eDate);
            insertCommand.Parameters.AddWithValue("@loca", eLocation);
            insertCommand.Parameters.AddWithValue("@particapants", eParticipants);
            insertCommand.Parameters.AddWithValue("@dura", eDura);

            insertCommand1.CommandText = "INSERT INTO eventsdatabase.eventtypes (name, eventtypeid ) VALUES (@type, @name, @eventtype, @name, 1);";

            if (insertCommand.ExecuteNonQuery() >= 1)
            {
                connection.Close();
                return true;
            }

            connection.Close();

            return false;
        }

        public static bool InsertAccounts(string nameUser, string phonenumber, string roleid, string password, string enabledUser)
        {
            connection.Open();

            MySqlCommand insertCommand = connection.CreateCommand();
            MySqlCommand insertCommand1 = connection.CreateCommand();


            insertCommand.CommandText = "INSERT INTO eventsdatabase.users (name, phonenumber, roleid, password, enabled) VALUES ( @name, @phone, @roleid, @password, @enabledUser);";
            insertCommand.Parameters.AddWithValue("@name", nameUser);
            insertCommand.Parameters.AddWithValue("@phone", phonenumber);
            insertCommand.Parameters.AddWithValue("@roleid", roleid);
            insertCommand.Parameters.AddWithValue("@password", password);
            insertCommand.Parameters.AddWithValue("@enabledUser", enabledUser);


            if (insertCommand.ExecuteNonQuery() >= 1)
            {
                connection.Close();
                return true;
            }
            connection.Close();
            return false;
        }

        public static bool InsertOR(string nameOR, string groupID, string infor)
        {
            connection.Open();

            MySqlCommand insertCommand = connection.CreateCommand();
            MySqlCommand insertCommand1 = connection.CreateCommand();


            insertCommand.CommandText = "INSERT INTO eventsdatabase.customergroups (name, groupid, info) VALUES ( @nameG, @groupid,@info);";
            insertCommand.Parameters.AddWithValue("@nameG", nameOR);
            insertCommand.Parameters.AddWithValue("@groupid", groupID);
            insertCommand.Parameters.AddWithValue("@info", infor);

            if (insertCommand.ExecuteNonQuery() >= 1)
            {
                connection.Close();
                return true;
            }
            connection.Close();
            return false;
        }






        public static void CloseConnection()
        {
            try
            {
                connection.Close();

            }
            catch (System.Exception)
            {
                // ERROR
            }
        }

        public static void LogActivity(string action, string status)
        {

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }


            MySqlCommand insertCommand = connection.CreateCommand();

            insertCommand.CommandText = "INSERT INTO eventsdatabase.logs (userid, action, target, status, datestamp) VALUES (@userid, @action, @target, @status, @datestamp);";
            insertCommand.Parameters.AddWithValue("@userid", 1);
            insertCommand.Parameters.AddWithValue("@action", action);
            insertCommand.Parameters.AddWithValue("@target", "events");
            insertCommand.Parameters.AddWithValue("@status", status);
            insertCommand.Parameters.AddWithValue("@datestamp", DateTime.UtcNow.ToString("o"));

            insertCommand.ExecuteNonQuery();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // ------------------ Janne's code begins
        public static DataTable GetDates(string _date1, string _date2)
        {
            /*          if (connection.State != ConnectionState.Open)
                     {
                         connection.Open();
                     }
                     MySqlCommand cmd = connection.CreateCommand();
                     cmd.CommandText = "SELECT * FROM eventsdatabase.events WHERE date BETWEEN @data1 AND @data2;";
                     cmd.Parameters.AddWithValue("@data1", _date1);
                     cmd.Parameters.AddWithValue("@data2", _date2);
                     var reader = cmd.ExecuteScalar();
                     DataTable results = new DataTable();
                     if (reader != null)
                     {

                     }
                     connection.Close();
                     return results; */

            if (String.IsNullOrEmpty(_date1) || _date1.Length < 2)
            {
                _date1 = "2015-01-01";
            }

            if (String.IsNullOrEmpty(_date2) || _date2.Length < 2)
            {
                _date2 = "2030-01-01";
            }

            DataTable results = new DataTable();
            MySqlCommand cmd = connection.CreateCommand();
            var adapter = new MySqlDataAdapter($"SELECT * FROM eventsdatabase.events WHERE date BETWEEN '{_date1}' AND '{_date2}';", connection);
            //   cmd.Parameters.AddWithValue("@data1", _date1);
            //   cmd.Parameters.AddWithValue("@data2", _date2);
            adapter.Fill(results);
            return results;

            /*    insertCommand.CommandText = "INSERT INTO eventsdatabase.logs (userid, action, target, status, datestamp) VALUES (@userid, @action, @target, @status, @datestamp);";
                insertCommand.Parameters.AddWithValue("@userid", 1);
                insertCommand.Parameters.AddWithValue("@action", action);
                insertCommand.Parameters.AddWithValue("@target", "events");
                insertCommand.Parameters.AddWithValue("@status", status); */


        }



        // ------------------ Janne's code ends










        // ------------------------ Antti's code begins



        public static DataTable GetAllUsers()
        {
            DataTable results = new DataTable();
            MySqlCommand cmd = connection.CreateCommand();
            var adapter = new MySqlDataAdapter("SELECT name,phonenumber,roleid,organizationr,enabled FROM eventsdatabase.users WHERE roleid < 950 ;", connection);
            adapter.Fill(results);
            return results;
        }


        public static DataTable GetAllPublicEventsTable()
        {
            DataTable results = new DataTable();
            MySqlCommand cmd = connection.CreateCommand();
            var adapter = new MySqlDataAdapter("SELECT name,eventtype,organizerid,date,participants FROM eventsdatabase.events WHERE enabled > 0;", connection);
            adapter.Fill(results);
            return results;
        }


        public static void UpdateCurrentUserInformation(string userName, string phoneNumber, string roleID, string organization, string enabled)
        {

            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(phoneNumber) || String.IsNullOrEmpty(roleID) || String.IsNullOrEmpty(organization) || String.IsNullOrEmpty(enabled))
            {
                return;
            }
                       
            
            Connection.LogActivity("Updating User:", userName + phoneNumber + roleID + organization + enabled);
            
            
            
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }




            MySqlCommand updateUserCommand = connection.CreateCommand();

            updateUserCommand.CommandText = $"UPDATE eventsdatabase.users SET phonenumber = @phonenumber , roleid = { Convert.ToInt32(roleID) }, organizationr = @organizationr , enabled = { Convert.ToInt32(enabled) }  WHERE name = ?username ;";

            updateUserCommand.Parameters.AddWithValue("?username", userName);
            updateUserCommand.Parameters.AddWithValue("@phonenumber", phoneNumber);
           // updateUserCommand.Parameters.AddWithValue("?roleid", Convert.ToInt32(roleID));
            updateUserCommand.Parameters.AddWithValue("@organizationr", organization);
           // updateUserCommand.Parameters.AddWithValue("?enabled", enabled);

            updateUserCommand.ExecuteNonQuery();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }


        public static DataTable GetAllLogs()
        {
            DataTable results = new DataTable();
            MySqlCommand cmd = connection.CreateCommand();
            var adapter = new MySqlDataAdapter("SELECT * FROM eventsdatabase.logs ;", connection);
            adapter.Fill(results);
            return results;
        }

        public  static void DeleteUserCompletely(string userName)
        {
            if (String.IsNullOrEmpty(userName))
            {
                return;
            }


            Connection.LogActivity("Deleting User:", userName);



            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }


            MySqlCommand updateUserCommand = connection.CreateCommand();

            updateUserCommand.CommandText = "DELETE FROM eventsdatabase.users WHERE name = ?username ;";
            updateUserCommand.Parameters.AddWithValue("?username", userName);

            updateUserCommand.ExecuteNonQuery();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }


        public  static void RegisterNewUser(string registerNewName, string registerPhoneNumber, string pass)
        {
            if (String.IsNullOrEmpty(registerNewName))
            {
                return;
            }


            Connection.LogActivity("New User:", registerNewName + registerPhoneNumber);



            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }


            MySqlCommand updateUserCommand = connection.CreateCommand();

            updateUserCommand.CommandText = "INSERT INTO eventsdatabase.users (name, phonenumber, password) VALUES ( ?username , ?phonenumber , ?password );";
            updateUserCommand.Parameters.AddWithValue("?username", registerNewName);
            updateUserCommand.Parameters.AddWithValue("?phonenumber", registerPhoneNumber);
            updateUserCommand.Parameters.AddWithValue("?password", pass);

            updateUserCommand.ExecuteNonQuery();

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }





        // ------------------------ Antti's code ends












        public static DataTable GetOR()
        {
            DataTable result = new DataTable();
            var adapter = new MySqlDataAdapter("SELECT OrganizationR , roleid FROM eventsdatabase.users WHERE roleid = 2;", connection);
            adapter.Fill(result);
            return result;
        }


    }
}
