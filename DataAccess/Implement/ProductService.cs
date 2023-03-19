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
    public class ProductService : IProductService
    {
        #region Declare
        private readonly IConfiguration configuration;
        private readonly ICategoryService categoryService;
        private readonly string dbconnection;
        private readonly string dateformat;
        #endregion
        #region Dependancy Injection
        public ProductService(IConfiguration configuration, ICategoryService categoryService)
        {
            this.configuration = configuration;
            this.categoryService = categoryService;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }
        #endregion
        #region Get
        public List<Product> GetProducts(string category, string subcategory, int count)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT TOP " + count + " * FROM Products WHERE CategoryId=(SELECT CategoryId FROM ProductCategories WHERE Category=@c AND SubCategory=@s) ORDER BY newid();";
                command.CommandText = query;
                command.Parameters.Add("@c", System.Data.SqlDbType.NVarChar).Value = category;
                command.Parameters.Add("@s", System.Data.SqlDbType.NVarChar).Value = subcategory;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"]
                    };

                    var categoryid = (int)reader["CategoryId"];
                    product.ProductCategory.Id = categoryid;

                    var offerid = (int)reader["OfferId"];
                    product.Offer.Id = offerid;

                    products.Add(product);
                }
                connection.Close();
            }
            return products;
        }
        public List<Product> GetProductsByQuantity(int count)
        {
            var result = new List<Product>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT TOP " + count + " * FROM Products ORDER BY CategoryId";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["ProductId"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"]
                    };

                    var categoryid = (int)reader["CategoryId"];
                    product.ProductCategory.Id = categoryid;

                    var offerid = (int)reader["OfferId"];
                    product.Offer.Id = offerid;

                    result.Add(product);
                }
                connection.Close();
            }
            return result;
        }
        public Product GetProduct(int id)
        {
            var product = new Product();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Products WHERE ProductId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    product.Id = (int)reader["ProductId"];
                    product.Title = (string)reader["Title"];
                    product.Description = (string)reader["Description"];
                    product.Price = (double)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                    product.ImageName = (string)reader["ImageName"];
                    product.ProductCategory.Id = (int)reader["CategoryId"];
                    product.Offer.Id = (int)reader["OfferId"];
                }
                connection.Close();
            }
            return product;
        }
        public Offer GetOffer(int id)
        {
            var offer = new Offer();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Offers WHERE OfferId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    offer.Id = (int)r["OfferId"];
                    offer.Title = (string)r["Title"];
                    offer.Discount = (int)r["Discount"]; ;
                }
                connection.Close();
            }
            return offer;
        }
        #endregion
        #region Insert

        public bool InsertProduct(UpdateProductReq product)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT Count(*) FROM Offers WHERE OfferId=" + product.OfferId + ";";

                connection.Open();
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                    if (count == 0)
                {
                    connection.Close();
                    return false;
                }
                query = "SELECT Count(*) FROM ProductCategories WHERE CategoryId=" + product.CategoryId + ";";
                command.CommandText = query;
                count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }
                query ="INSERT INTO Products(Title, Description, CategoryId, OfferId, Price, Quantity, ImageName)" +
                       "VALUES(" + "N'" + product.Title + "'" +
                       ", N'" + product.Description + "'," + product.CategoryId + "," +
                       product.OfferId + "," + product.Price + "," + product.Quantity +
                       ",'" + product.ImageName + "');";
                command.CommandText = query;
                count = command.ExecuteNonQuery();
                if (count <= 0)
                {
                    connection.Close();
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Update

        public bool UpdateProduct(UpdateProductReq req)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Products WHERE ProductId=" + req.Id;
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }
                query = "SELECT COUNT(*) FROM ProductCategories WHERE CategoryId=" + req.CategoryId + ";";
                command.CommandText = query;
                count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }
                query = "SELECT COUNT(*) FROM Offers WHERE OfferId=" + req.OfferId + ";";
                command.CommandText = query;
                count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }
                query = "UPDATE Products " +
                        "SET Title=" + "N'" + req.Title + "'" +
                        ",Description=" + "N'" + req.Description + "'" +
                        ", CategoryId=" + req.CategoryId +
                        ", OfferId="+ req.OfferId +
                        ",Price=" + req.Price +
                        ",Quantity=" + req.Quantity +
                        ",ImageName=" + "'" + req.ImageName + "'" +
                        "WHERE ProductId=" + req.Id;
                command.CommandText = query;
                command.ExecuteNonQuery();
                if (count <= 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
            }
            return true;
        }
        #endregion
        #region Delete
        public bool DeleteProduct(int id)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Products WHERE ProductId=" + id;
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    return false;
                }
                query = "DELETE FROM Products WHERE ProductId=" + id;
                command.CommandText = query;
                command.ExecuteNonQuery();

                connection.Close();
            }
            return true;
        }
        #endregion
        
    }
}

