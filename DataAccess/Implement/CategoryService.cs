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
    public class CategoryService : ICategoryService
    {
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;
        public CategoryService(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }
        public List<ProductCategory> GetProductCategories()
        {
            var productCategories = new List<ProductCategory>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM ProductCategories;";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = new ProductCategory()
                    {
                        Id = (int)reader["CategoryId"],
                        Category = (string)reader["Category"],
                        SubCategory = (string)reader["SubCategory"]
                    };
                    productCategories.Add(category);
                }
            }
            return productCategories;
        }
        public ProductCategory GetProductCategory(int id)
        {
            var productCategory = new ProductCategory();

            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM ProductCategories WHERE CategoryId=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    productCategory.Id = (int)r["CategoryId"];
                    productCategory.Category = (string)r["Category"];
                    productCategory.SubCategory = (string)r["SubCategory"];
                }
            }

            return productCategory;
        }
        public bool InsertCategory(ProductCategory category)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) FROM ProductCategories WHERE CategoryId='" + category.Id + "';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    connection.Close();
                    return false;
                }

                query = "INSERT INTO ProductCategories (Category,Subcategory) values (@cat, @sub);";

                command.CommandText = query;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = category.Category;
                command.Parameters.Add("@sub", System.Data.SqlDbType.NVarChar).Value = category.SubCategory;

                command.ExecuteNonQuery();
            }
            return true;
        }
        public bool UpdateCategory(ProductCategory id)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();


                string query = "SELECT COUNT(*) FROM ProductCategories WHERE CategoryId ='" + id.Id + "' ;";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }


                query = "UPDATE ProductCategories " +
                               "SET  Category=@cat, SubCategory=@sub WHERE CategoryId=" + id.Id + ";";

                command.CommandText = query;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = id.Category;
                command.Parameters.Add("@sub", System.Data.SqlDbType.NVarChar).Value = id.SubCategory;

                command.ExecuteNonQuery();
            }
            return true;
        }
        public bool DeleteCategory(ProductCategory id)
        {
            var users = new List<User>();
            using (SqlConnection connection = new(dbconnection)) // ket noi database
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();
                string query = "SELECT COUNT(*) FROM ProductCategories WHERE CategoryId ='" + id.Id + "' ;";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }

                query = "DELETE FROM ProductCategories WHERE CategoryId ='" + id.Id + "' ;";
                command.CommandText = query;


                SqlDataReader reader = command.ExecuteReader();
            
            }
            return true;
        }
    }
}


