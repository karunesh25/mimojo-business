using MySqlConnector;
using System;
using Mimojo.Business.Model;
using System.Text;

namespace Mimojo.Business.Dal
{
    public static class RegistrationDal
    {
        public static async void Create(RegistrationModel registration)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_registration",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {

                    command.CommandText = @"INSERT INTO mimojo_registration.user_register (userid, password, emailid, name, mobile, country, city, address) 
                                                 VALUES (@userid, @password, @emailid, @name, @mobile, @country, @city, @address);";
                    command.Parameters.AddWithValue("@userid", registration.UserId);
                    command.Parameters.AddWithValue("@password", Convert.ToBase64String(Encoding.UTF8.GetBytes(registration.Password)));// Encoding.UTF8.GetString(Convert.FromBase64String(encodedStr));
                    command.Parameters.AddWithValue("@emailid", registration.EmailId);
                    command.Parameters.AddWithValue("@name", registration.Name);
                    command.Parameters.AddWithValue("@mobile", registration.Mobile);
                    command.Parameters.AddWithValue("@country", registration.Country);
                    command.Parameters.AddWithValue("@city", registration.City);
                    command.Parameters.AddWithValue("@address", registration.Address);

                    int rowCount = await command.ExecuteNonQueryAsync();
                    Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                }

                // connection will be closed by the 'using' block
                Console.WriteLine("Closing connection");
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }

        public static LoginResponseModel GetLoginDetails(LoginModel model)
        {
            LoginResponseModel retVal = new LoginResponseModel() { IsValidUser = false };
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_registration",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                conn.Open();

                string sql = string.Format("SELECT userid, password FROM mimojo_registration.user_register where userid = '{0}' and password = '{1}';", model.UserId, Convert.ToBase64String(Encoding.UTF8.GetBytes(model.Password)));
                using var cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    retVal = GetData(rdr);
                }
            }
            return retVal;
        }

        private static LoginResponseModel GetData(MySqlDataReader rdr)
        {
            LoginResponseModel model = new LoginResponseModel();
            model.UserId = rdr.GetString(0);
            model.IsValidUser = true;
            model.AccessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.UserId));
            return model;
        }
        public static RegistrationModel GetRegistrationDetails(ProfileRequestModel model)
        {
            RegistrationModel retVal = new RegistrationModel();
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_registration",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                conn.Open();

                string sql = string.Format("SELECT userid, emailid, name, mobile, country, city, address FROM mimojo_registration.user_register where userid = '{0}';", model.UserId);
                using var cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    retVal = GetRegistrationData(rdr);
                }
            }
            return retVal;
        }
        private static RegistrationModel GetRegistrationData(MySqlDataReader rdr)
        {
            RegistrationModel model = new RegistrationModel();
            model.UserId = Convert.IsDBNull(rdr["userid"]) ? "" : rdr.GetString("userid");
            model.EmailId = Convert.IsDBNull(rdr["emailid"]) ? "" : rdr.GetString("emailid");
            model.Name = Convert.IsDBNull(rdr["name"]) ? "" : rdr.GetString("name");
            model.Mobile = Convert.IsDBNull(rdr["mobile"]) ? "" : rdr.GetString("mobile");
            model.Country = Convert.IsDBNull(rdr["country"]) ? "" : rdr.GetString("country");
            model.City = Convert.IsDBNull(rdr["city"]) ? "" : rdr.GetString("city");
            model.Address = Convert.IsDBNull(rdr["address"]) ? "" : rdr.GetString("address");
            return model;
        }

    }
}
