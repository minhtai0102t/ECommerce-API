using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using ECommerce.API.Models.Request;
using System.Data.SqlClient;
namespace ECommerce.API.DataAccess
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;
        public PaymentService(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }

        public List<Payment> GetPayments()
        {
            var payments = new List<Payment>();
            using (SqlConnection connection = new SqlConnection(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Payments";
                connection.Open();
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    var payment = new Payment();
                    payment.Id = (int)reader["PaymentId"];
                    payment.User.Id = (int)reader["UserId"];
                    payment.PaymentMethod.Id = (int)reader["PaymentMethodId"];
                    payment.TotalAmount = (int)reader["TotalAmount"];
                    payment.ShipingCharges = (int)reader["ShipingCharges"];
                    payment.AmountReduced = (int)reader["AmountReduced"];
                    payment.AmountPaid = (int)reader["AmountPaid"];
                    payment.CreatedAt = (string)reader["CreatedAt"];
                    payments.Add(payment);
                }
            }
            return payments;
        }
        public Payment GetPaymentById(int id)
        {
            var payment = new Payment();
            using (SqlConnection connection = new SqlConnection(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Payments WHERE Id=" + id + ";";
                connection.Open();
                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    payment.Id = (int)reader["Id"];
                    payment.User.Id = (int)reader["UserId"];
                    payment.PaymentMethod.Id = (int)reader["PaymentMethodId"];
                    payment.TotalAmount = (int)reader["TotalAmount"];
                    payment.ShipingCharges = (int)reader["ShipingCharges"];
                    payment.AmountReduced = (int)reader["AmountReduced"];
                    payment.AmountPaid = (int)reader["AmountPaid"];
                    payment.CreatedAt = (string)reader["CreatedAt"];
                }
            }
            return payment;
        }
        public bool InsertPayment(InsertPaymentReq payment)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT CartId FROM Carts WHERE UserId=" + payment.UserId + ";";
                connection.Open();
                command.CommandText = query;
                var checkCartId = command.ExecuteScalar();
                if(checkCartId == null)
                {
                    connection.Close();
                    return false;
                }

                query = @"INSERT INTO Payments (PaymentMethodId, UserId, TotalAmount, ShippingCharges, AmountReduced, AmountPaid, CreatedAt) 
                                VALUES (@pmid, @uid, @ta, @sc, @ar, @ap, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@pmid", System.Data.SqlDbType.Int).Value = payment.PaymentMethodId;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = payment.UserId;
                command.Parameters.Add("@ta", System.Data.SqlDbType.NVarChar).Value = payment.TotalAmount;
                command.Parameters.Add("@sc", System.Data.SqlDbType.NVarChar).Value = payment.ShipingCharges;
                command.Parameters.Add("@ar", System.Data.SqlDbType.NVarChar).Value = payment.AmountReduced;
                command.Parameters.Add("@ap", System.Data.SqlDbType.NVarChar).Value = payment.AmountPaid;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = DateTime.Now;

                int value = command.ExecuteNonQuery();
                if (value <= 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();

            }
            return true;
        }

        public bool UpdatePayment(int id,UpdatePaymentReq payment)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Payments WHERE PaymentId=" + id + ";";
                connection.Open();
                command.CommandText = query;
                var check = command.ExecuteScalar();
                // Check if paymentId not exists 
                if (check == null)
                {
                    connection.Close();
                    return false;
                }

                query = @"UPDATE Payments (PaymentMethodId, TotalAmount, ShippingCharges, AmountReduced, AmountPaid, CreatedAt) 
                                SET PaymentMethodId=@pmid, TotalAmount=@ta, ShippingCharges=@sc, AmountReduced=@ar, AmountPaid=@ap, CreatedAt=@cat) 
                                WHERE PaymentId=" + id + ";";

                command.CommandText = query;
                command.Parameters.Add("@pmid", System.Data.SqlDbType.Int).Value = payment.PaymentMethodId;
                command.Parameters.Add("@ta", System.Data.SqlDbType.NVarChar).Value = payment.TotalAmount;
                command.Parameters.Add("@sc", System.Data.SqlDbType.NVarChar).Value = payment.ShipingCharges;
                command.Parameters.Add("@ar", System.Data.SqlDbType.NVarChar).Value = payment.AmountReduced;
                command.Parameters.Add("@ap", System.Data.SqlDbType.NVarChar).Value = payment.AmountPaid;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = DateTime.Now;

                connection.Open();
                int value = command.ExecuteNonQuery();
                if (value <= 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
            }
            return true;
        }
        public bool DeletePayment(int id)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "DELETE FROM Orders WHERE PaymentId=" + id + ";";
                connection.Open();
                command.CommandText = query;
                command.ExecuteNonQuery();

                query = "DELETE FROM Payments WHERE PaymentId=" + id + ";";
                command.CommandText = query;
                int count = command.ExecuteNonQuery();
                if(count <=0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
                return true;
            }
        }
    }
}

