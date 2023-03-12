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
		private readonly IConfiguration configuration;
		private readonly ICategoryService categoryService;
        private readonly string dbconnection;
        private readonly string dateformat;
        public ProductService(IConfiguration configuration, ICategoryService categoryService)
        {
            this.configuration = configuration;
            this.categoryService = categoryService;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
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

                    var categoryid = (int)reader["CategoryId"];
                    product.ProductCategory = categoryService.GetProductCategory(categoryid);

                    var offerid = (int)reader["OfferId"];
                    product.Offer = GetOffer(offerid);
                }
            }
            return product;
        }
        public bool InsertProduct(UpdateProductReq product)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string isProductIdExisted = "SELECT Count(*) FROM Products WHERE ProductId=" + product.Id + ";";
                string isOfferIdExisted = "SELECT Count(*) FROM Offers WHERE OfferId=" + product.OfferId + ";";
                string isCategoryIdExisted = "SELECT Count(*) FROM ProductCategories WHERE CategoryId=" + product.CategoryId + ";";

                command.CommandText = isProductIdExisted;
                int countProduct = (int)command.ExecuteScalar();
                if (countProduct != 0)
                {
                    return false;
                }

                command.CommandText = isOfferIdExisted;
                int countOffer = (int)command.ExecuteScalar();
                if (countOffer == 0)
                {
                    return false;
                }

                command.CommandText = isCategoryIdExisted;
                int countCategory = (int)command.ExecuteScalar();
                if (countCategory == 0)
                {
                    return false;
                }
                string insertQuery = "SET IDENTITY_INSERT Products ON;" + "INSERT INTO Products(ProductId, Title, Description, CategoryId, OfferId, Price, Quantity, ImageName)" +
                       "VALUES(" + product.Id +
                       ",'" + product.Title + "'" +
                       ",'" + product.Description + "'," + product.CategoryId + "," +
                       product.OfferId + "," + product.Price + "," + product.Quantity +
                       ",'" + product.ImageName + "');"
                       + "SET IDENTITY_INSERT Products OFF;";

                command.CommandText = insertQuery;
                int res = command.ExecuteNonQuery();
                if (res <= 0) return false;
            }
               
            return true;
        }

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
                    return false;
                }
                query = "UPDATE Products " +
                        "SET Title=" + "'" + req.Title + "'" + 
                        ",Description=" + "'" + req.Description + "'" +
                        ",Price=" + req.Price +
                        ",Quantity=" + req.Quantity +
                        ",ImageName=" + "'" + req.ImageName + "'" +
                        "WHERE ProductId=" + req.Id;
                command.CommandText = query;
                command.ExecuteNonQuery();
                return true;
            }
        }
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
                    product.ProductCategory = categoryService.GetProductCategory(categoryid);

                    var offerid = (int)reader["OfferId"];
                    product.Offer = GetOffer(offerid);

                    products.Add(product);
                }
            }
            return products;
        }
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
                return true;
            }
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
                    offer.Discount = (int)r["Discount"];
                }
            }
            return offer;
        }
	}
}

