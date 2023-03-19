using System;
using ECommerce.API.Models;
using ECommerce.API.Models.Request;

namespace ECommerce.API.DataAccess
{
	public interface IProductService
	{
        bool InsertProduct(UpdateProductReq product);
        List<Product> GetProducts(string category, string subcategory, int count); 
        List<Product> GetProductsByQuantity(int count);
        Product GetProduct(int id);
        bool UpdateProduct(UpdateProductReq id);
        bool DeleteProduct(int id);
		//Offder
        Offer GetOffer(int id);
	}
}

