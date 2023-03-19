using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface ICategoryService : IDisposable
	{
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategory(int id);
        bool InsertCategory(ProductCategory category);
        bool UpdateCategory(ProductCategory category);
        bool DeleteCategory(int id);
    }
}

