using Mimojo.Business.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mimojo.Business.Dal
{
    public static class PaymentDal
    {
        public static async void Create(PaymentModel payment)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_payment",
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

                    command.CommandText = @"INSERT INTO mimojo_payment.user_payment (userid, cardbin, cardlast4, expiry, cardname, amount, currency, transdate) 
                                                 VALUES (@userid, @cardbin, @cardlast4, @expiry, @cardname, @amount, @currency, @transdate);";
                    command.Parameters.AddWithValue("@userid", payment.UserId);
                    command.Parameters.AddWithValue("@cardbin", payment.CardBin);
                    command.Parameters.AddWithValue("@cardlast4", payment.CardLast4);
                    command.Parameters.AddWithValue("@expiry", payment.Expiry);
                    command.Parameters.AddWithValue("@cardname", payment.CardName);
                    command.Parameters.AddWithValue("@amount", payment.Amount);
                    command.Parameters.AddWithValue("@currency", payment.Currency);
                    command.Parameters.AddWithValue("@transdate", payment.Date == DateTime.MinValue ? DateTime.Now : payment.Date);

                    int rowCount = await command.ExecuteNonQueryAsync();
                    Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                }

                // connection will be closed by the 'using' block
                Console.WriteLine("Closing connection");
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }

        public static PaymentModel GetPaymentDetails(ProfileRequestModel model)
        {
            PaymentModel retVal = new PaymentModel();
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mimojodb.mysql.database.azure.com",
                Database = "mimojo_payment",
                UserID = "hkarunesh25@mimojodb",
                Password = "abcd@123456",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                conn.Open();

                string sql = string.Format("SELECT userid, cardbin, cardlast4, expiry, cardname, amount, currency, transdate FROM mimojo_payment.user_payment where userid = '{0}';", model.UserId);
                using var cmd = new MySqlCommand(sql, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    retVal = GetPaymentData(rdr);
                }
            }
            return retVal;
        }
        private static PaymentModel GetPaymentData(MySqlDataReader rdr)
        {
            PaymentModel model = new PaymentModel();
            model.UserId = Convert.IsDBNull(rdr["userid"]) ? "" : rdr.GetString("userid");
            model.CardBin = Convert.IsDBNull(rdr["cardbin"]) ? "" : rdr.GetString("cardbin");
            model.CardLast4 = Convert.IsDBNull(rdr["cardlast4"]) ? "" : rdr.GetString("cardlast4");
            model.Expiry = Convert.IsDBNull(rdr["expiry"]) ? "" : rdr.GetString("expiry");
            model.CardName = Convert.IsDBNull(rdr["cardname"]) ? "" : rdr.GetString("cardname");
            model.Amount = Convert.IsDBNull(rdr["amount"]) ? 0 : rdr.GetDecimal("amount");
            model.Currency = Convert.IsDBNull(rdr["currency"]) ? "" : rdr.GetString("currency");
            model.Date = Convert.IsDBNull(rdr["transdate"]) ? DateTime.MinValue : rdr.GetDateTime("transdate");
            return model;
        }
    }
}
