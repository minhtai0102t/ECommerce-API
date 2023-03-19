using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using ECommerce.API.Models.Request;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.API.DataAccess
{
    public class CartService : ICartService
    {
        #region Declare
        private readonly IConfiguration configuration;
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly string dbconnection;
        private readonly string dateformat;
        #endregion
        #region Dependancy Injection
        public CartService(IConfiguration configuration, IProductService productService, IUserService userService)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
            this.userService = userService;
            this.productService = productService;
        }
        #endregion
        #region Get
        public Cart GetActiveCartOfUser(int userid)
        {
            var cart = new Cart();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) From Carts WHERE UserId=" + userid + " AND Ordered='false';";
                command.CommandText = query;

                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    return cart;
                }

                query = "SELECT CartId From Carts WHERE UserId=" + userid + " AND Ordered='false';";
                command.CommandText = query;

                int cartid = (int)command.ExecuteScalar();

                query = "SELECT * From CartItems WHERE CartId=" + cartid + ";";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartItem item = new()
                    {
                        Id = (int)reader["CartItemId"],
                        Product = productService.GetProduct((int)reader["ProductId"]),
                        Quantity = (int)reader["quantity"]
                    };
                    cart.CartItems.Add(item);
                }

                cart.Id = cartid;
                cart.User = userService.GetUser(userid);
                cart.Ordered = false;
                cart.OrderedOn = "";

                connection.Close();
            }
            return cart;
        }
        public Cart GetCart(int cartid)
        {
            var cart = new Cart();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartItem item = new()
                    {
                        Id = (int)reader["CartItemId"],
                        Product = productService.GetProduct((int)reader["ProductId"]),
                        Quantity = (int)reader["quantity"]
                    };
                    cart.CartItems.Add(item);
                }
                reader.Close();

                query = "SELECT * FROM Carts WHERE CartId=" + cartid + ";";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cart.Id = cartid;
                    cart.User = userService.GetUser((int)reader["UserId"]);
                    cart.Ordered = bool.Parse((string)reader["Ordered"]);
                    cart.OrderedOn = (string)reader["OrderedOn"];
                }
                reader.Close();
                connection.Close();
            }
            return cart;
        }
        public List<Cart> GetAllPreviousCartsOfUser(int userid)
        {
            var carts = new List<Cart>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT CartId FROM Carts WHERE UserId=" + userid + " AND Ordered='true';";
                command.CommandText = query;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var cartid = (int)reader["CartId"];
                    carts.Add(GetCart(cartid));
                }
                connection.Close();
            }
            return carts;
        }
        #endregion
        #region Insert
        public bool InsertCartItem(int userId, int productId, int quantity)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    query = "INSERT INTO Carts (UserId, Ordered, OrderedOn) VALUES (" + userId + ", 'false', '');";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                query = "SELECT CartId FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int cartId = (int)command.ExecuteScalar();

                query = "SELECT COUNT(*) FROM CartItems WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                command.CommandText = query;
                int isCartIdExisted = (int)command.ExecuteScalar();
                if (isCartIdExisted == 0)
                {
                    query = "INSERT INTO CartItems (CartId, ProductId, Quantity) VALUES (" + cartId + ", " + productId + ", " + quantity + ");";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                else
                {
                    query = "SELECT TOP 1 Quantity FROM CartItems WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();

                    int dbQuantity = -1;
                    while (reader.Read())
                    {
                        dbQuantity = (int)reader["Quantity"];
                    }
                    reader.Close();

                    if (dbQuantity == -1)
                    {
                        return false;
                    }

                    query = "UPDATE CartItems SET Quantity=" + (dbQuantity + quantity) + " WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    connection.Close();
                }
                return true;
            }
        }
        #endregion
        #region Delete
        public bool DeleteCartItem(int userId, int productId, int quantity)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    return false;
                }

                query = "SELECT CartId FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int cartId = (int)command.ExecuteScalar();

                query = "SELECT COUNT(*) FROM CartItems WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                command.CommandText = query;
                int isCartIdExisted = (int)command.ExecuteScalar();
                if (isCartIdExisted == 0)
                {
                    return false;
                }
                else
                {
                    query = "SELECT TOP 1 Quantity FROM CartItems WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader();

                    int dbQuantity = -1;
                    while (reader.Read())
                    {
                        dbQuantity = (int)reader["Quantity"];
                    }
                    reader.Close();
                    if (dbQuantity == -1 || quantity > dbQuantity)
                    {
                        return false;
                    }
                    query = "UPDATE CartItems SET Quantity=" + (dbQuantity - quantity) + " WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    connection.Close();

                }
                return true;
            }
        }
        #endregion
    }
}

