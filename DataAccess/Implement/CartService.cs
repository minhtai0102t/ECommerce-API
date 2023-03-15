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
		private readonly IConfiguration configuration;
        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly string dbconnection;
        private readonly string dateformat;
        public CartService(IConfiguration configuration, IProductService productService, IUserService userService)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
            this.userService = userService;
            this.productService = productService;
        }
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

                    if(dbQuantity == -1)
                    {
                        return false;
                    }
                    
                    query = "UPDATE CartItems SET Quantity=" + (dbQuantity + quantity) + " WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    
                }
                return true;
            }
        }

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
                    if(dbQuantity == -1 || quantity > dbQuantity)
                    {
                        return false;
                    }
                    query = "UPDATE CartItems SET Quantity=" + (dbQuantity - quantity) + " WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                return true;
            }
        }

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
            }
            return carts;
        }
        public bool DeleteCartItem(int userId,int productId)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();


                string query = "SELECT CartId FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int cartId = (int)command.ExecuteScalar();
                if(cartId == 0)
                {
                    return false;
                }

                query = "DELETE FROM CartItems WHERE CartId=" + cartId + "AND ProductId=" + productId + ";";
                return true;

            }

        }
    }
}

