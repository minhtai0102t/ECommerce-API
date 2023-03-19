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
        #region Declare
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
        private readonly string dateformat;
        #endregion
        #region Dependancy Injection
        public CategoryService(IConfiguration configuration)
        {
            this.configuration = configuration;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }
        #endregion
        #region Get
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
                connection.Close();
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
                connection.Close();
            }

            return productCategory;
        }
        #endregion
        #region Insert

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
                connection.Close();
            }
            return true;
        }
        #endregion
        #region Update
        public bool UpdateCategory(ProductCategory category)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();


                string query = "SELECT COUNT(*) FROM ProductCategories WHERE CategoryId ='" + category.Id + "' ;";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }


                query = "UPDATE ProductCategories " +
                               "SET  Category=@cat, SubCategory=@sub WHERE CategoryId=" + category.Id + ";";

                command.CommandText = query;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = category.Category;
                command.Parameters.Add("@sub", System.Data.SqlDbType.NVarChar).Value = category.SubCategory;

                command.ExecuteNonQuery();
                connection.Close();
            }
            return true;
        }
        #endregion
        #region Delete
        public bool DeleteCategory(int id)
        {
            using (SqlConnection connection = new(dbconnection)) // ket noi database
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();
                string query = "SELECT COUNT(*) FROM ProductCategories WHERE CategoryId ='" + id + "' ;";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    connection.Close();
                    return false;
                }

                query = "DELETE FROM ProductCategories WHERE CategoryId ='" + id + "' ;";
                command.CommandText = query;
                int total = (int)command.ExecuteNonQuery();
                if (total <= 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
            }
            return true;
        }
        #endregion

        #region Destructor
        public void Dispose()
        {
        }
        #endregion Destructor
    }
}

