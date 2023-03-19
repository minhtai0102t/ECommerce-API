using System;
using ECommerce.API.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace ECommerce.API.DataAccess
{
	public class PaymentMethodService : IPaymentMethodService
	{
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;
        public PaymentMethodService(IConfiguration configuration)
		{
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }
        #region Get
        public List<PaymentMethod> GetPaymentMethods()
        {
            var result = new List<PaymentMethod>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM PaymentMethods;";
                command.CommandText = query;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PaymentMethod paymentMethod = new()
                    {
                        Id = (int)reader["PaymentMethodId"],
                        Type = (string)reader["Type"],
                        Provider = (string)reader["Provider"],
                        Available = bool.Parse((string)reader["Available"]),
                        Reason = (string)reader["Reason"]
                    };
                    result.Add(paymentMethod);
                }
                connection.Close();
            }
            return result;
        }
        public PaymentMethod GetPaymentMethodById(int id)
        {
            var result = new PaymentMethod();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM PaymentMethods WHERE PaymentMethodId=" + id + " ;";
                command.CommandText = query;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Id = (int)reader["PaymentMethodId"];
                    result.Type = (string)reader["Type"];
                    result.Provider = (string)reader["Provider"];
                    result.Available = bool.Parse((string)reader["Available"]);
                    result.Reason = (string)reader["Reason"];
                }
                connection.Close();
            }
            return result;
        }
        #endregion

        #region Insert
        public bool InsertPaymentMethod(PaymentMethod method)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT Count(*) FROM PaymentMethods WHERE PaymentMethodId= " + method.Id + " ;";
                command.CommandText = query;

                connection.Open();
                int count = (int)command.ExecuteScalar();
                if(count > 0)
                {
                    // existed
                    connection.Close();
                    return false;

                }
                // not exsits then insert
                query = "INSERT INTO PaymentMethods VALUES (@type, @provider, @available, @reason);";

                command.Parameters.Add("@type", System.Data.SqlDbType.NVarChar).Value = method.Type;
                command.Parameters.Add("@provider", System.Data.SqlDbType.NVarChar).Value = method.Provider;
                command.Parameters.Add("@available", System.Data.SqlDbType.NVarChar).Value = method.Available;
                command.Parameters.Add("@reason", System.Data.SqlDbType.NVarChar).Value = method.Reason;

                command.CommandText = query;
                command.ExecuteNonQuery();
                connection.Close();
            }
            return true;
        }
        #endregion

        #region Update
        public bool UpdatePaymentMethod(PaymentMethod method)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT Count(*) FROM PaymentMethods WHERE PaymentMethodId= " + method.Id + " ;";
                command.CommandText = query;

                connection.Open();
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    // not existed
                    connection.Close();
                    return false;

                }
                // exsits then update
                query = "UPDATE PaymentMethods " +
                        "SET Type=@type, Provider=@provider, Available=@available, Reason=@reason " +
                        "WHERE PaymentMethodId=@id;";
                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = method.Id;
                command.Parameters.Add("@type", System.Data.SqlDbType.NVarChar).Value = method.Type;
                command.Parameters.Add("@provider", System.Data.SqlDbType.NVarChar).Value = method.Provider;
                command.Parameters.Add("@available", System.Data.SqlDbType.NVarChar).Value = method.Available;
                command.Parameters.Add("@reason", System.Data.SqlDbType.NVarChar).Value = method.Reason;
                command.CommandText = query;

                command.ExecuteNonQuery();
                connection.Close();
            }
            return true;
        }
        #endregion

        #region Delete
        public bool DeletePaymentMethod(int id)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();
                string query = "DELETE FROM PaymentMethods WHERE PaymentMethodId= " + id + " ;";
                command.CommandText = query;
                int count = (int)command.ExecuteNonQuery();
                if (count == 0)
                {
                    // not existed
                    connection.Close();
                    return false;

                }
                connection.Close();
            }
            return true;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {

        }
        #endregion
    }
}

