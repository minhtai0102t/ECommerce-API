using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface ICategoryService
	{
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategory(int id);
		bool InsertCategory (ProductCategory category);
        bool UpdateCategory(ProductCategory id);
        bool DeleteCategory(ProductCategory id);

    }
}

