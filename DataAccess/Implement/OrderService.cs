using ECommerce.API.Models;
using System.Data.SqlClient;
namespace ECommerce.API.DataAccess
{
    public class OrderService : IOrderService
    {
        #region Declare

        #endregion
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;
        public OrderService(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }
        public List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM Orders;";
                command.CommandText = query;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Order order = new Order();
                    order.Id = (int)reader["Id"];
                    order.User.Id = (int)reader["UserId"];
                    order.Cart.Id = (int)reader["CartId"];
                    order.Payment.Id = (int)reader["PaymentId"];
                    order.CreatedAt = (string)reader["CreatedAt"];

                    orders.Add(order);
                }
                return orders;
            }
        }
        public Order GetOrderById(int id)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                Order order = new Order();
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM Orders WHERE Id=" + id + ";";
                command.CommandText = query;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    order.Id = (int)reader["Id"];
                    order.User.Id = (int)reader["UserId"];
                    order.Cart.Id = (int)reader["CartId"];
                    order.Payment.Id = (int)reader["PaymentId"];
                    order.CreatedAt = (string)reader["CreatedAt"];
                }
                return order;
            }
        }
        public int InsertOrder(InsertOrderReq order)
        {
            int value = 0;

            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "INSERT INTO Orders (UserId, CartId, PaymentId, CreatedAt) values (@uid, @cid, @pid, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = order.UserId;
                command.Parameters.Add("@cid", System.Data.SqlDbType.Int).Value = order.CartId;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = order.CreatedAt;
                command.Parameters.Add("@pid", System.Data.SqlDbType.Int).Value = order.PaymentId;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "UPDATE Carts SET Ordered='true', OrderedOn='" + DateTime.Now.ToString(dateformat) + "' WHERE CartId=" + order.CartId + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    query = "SELECT TOP 1 Id FROM Orders ORDER BY Id DESC;";
                    command.CommandText = query;
                    value = (int)command.ExecuteScalar();
                }
                else
                {
                    value = 0;
                }
                connection.Close();
            }
            return value;
        }
        public bool DeleteOrder(int id)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "DELETE Orders WHERE Id=" + id + ";";
                command.CommandText = query;
                connection.Open();
                int count = command.ExecuteNonQuery();
                if(count == 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
            }
                
            return true;
        }

    }
}

