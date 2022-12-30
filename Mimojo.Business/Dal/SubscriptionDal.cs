using Mimojo.Business.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Dal
{
    public static class SubscriptionDal
    {
        public static async void Create(SubscriptionModel subs)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_subscription",
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

                    command.CommandText = @"INSERT INTO mimojo_subscription.user_subscription (userid, subscription, description, purchasedate, expirydate) 
                                                 VALUES (@userid, @subscription, @description, @purchasedate, @expirydate);";
                    command.Parameters.AddWithValue("@userid", subs.UserId);
                    command.Parameters.AddWithValue("@subscription", subs.Subscription);
                    command.Parameters.AddWithValue("@description", subs.Description);
                    command.Parameters.AddWithValue("@purchasedate", subs.PurchaseDate == DateTime.MinValue ? DateTime.Now : subs.PurchaseDate);
                    command.Parameters.AddWithValue("@expirydate", subs.ExpiryDate == DateTime.MinValue ? DateTime.Now.AddMonths(1) : subs.ExpiryDate);

                    int rowCount = await command.ExecuteNonQueryAsync();
                    Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                }

                // connection will be closed by the 'using' block
                Console.WriteLine("Closing connection");
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
        public static SubscriptionModel GetSubscriptionDetails(ProfileRequestModel model)
        {
            SubscriptionModel retVal = new SubscriptionModel();
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_subscription",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                conn.Open();

                string sql = string.Format("SELECT userid, subscription, description, purchasedate, expirydate FROM mimojo_subscription.user_subscription where userid = '{0}';", model.UserId);
                using var cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    retVal = GetSubscriptionData(rdr);
                }
            }
            return retVal;
        }
        private static SubscriptionModel GetSubscriptionData(MySqlDataReader rdr)
        {
            SubscriptionModel model = new SubscriptionModel();
            model.UserId = Convert.IsDBNull(rdr["userid"]) ? "" : rdr.GetString("userid");
            model.Subscription = Convert.IsDBNull(rdr["subscription"]) ? "" : rdr.GetString("subscription");
            model.Description = Convert.IsDBNull(rdr["description"]) ? "" : rdr.GetString("description");
            model.PurchaseDate = Convert.IsDBNull(rdr["purchasedate"]) ? DateTime.MinValue : rdr.GetDateTime("purchasedate");
            model.ExpiryDate = Convert.IsDBNull(rdr["expirydate"]) ? DateTime.MinValue : rdr.GetDateTime("expirydate");
            return model;
        }


        public static List<CountryModel> GetCountries()
        {
            List<CountryModel> retVal = new List<CountryModel>();
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_subscription",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                conn.Open();

                string sql = string.Format("SELECT id, country FROM mimojo_subscription.country;");
                using var cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    retVal.Add(GetCountry(rdr));
                }
            }
            return retVal;
        }

        public static List<CountryModel> GetUserCountries(ProfileRequestModel model)
        {
            List<CountryModel> retVal = new List<CountryModel>();
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_subscription",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                conn.Open();

                string sql = string.Format("SELECT id, country FROM mimojo_subscription.user_subs_country where userid = '{0}';", model.UserId);
                using var cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    retVal.Add(GetCountry(rdr));
                }
            }
            return retVal;
        }

        private static CountryModel GetCountry(MySqlDataReader rdr)
        {
            CountryModel model = new CountryModel();
            model.Id = Convert.IsDBNull(rdr["id"]) ? 0 : rdr.GetInt16("id");
            model.Country = Convert.IsDBNull(rdr["country"]) ? "" : rdr.GetString("country");
            return model;
        }

        public static async void CreateUserCountry(CountryUserModel subs)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_subscription",
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

                    command.CommandText = @"INSERT INTO mimojo_subscription.user_subs_country (userid, country) 
                                                 VALUES (@userid, @country);";
                    command.Parameters.AddWithValue("@userid", subs.UserId);
                    command.Parameters.AddWithValue("@country", subs.Country);

                    int rowCount = await command.ExecuteNonQueryAsync();
                    Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                }

                // connection will be closed by the 'using' block
                Console.WriteLine("Closing connection");
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
    }
}
