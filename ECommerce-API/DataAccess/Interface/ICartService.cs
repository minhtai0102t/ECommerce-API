using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface ICartService
	{
        bool InsertCartItem(int userId, int productId);
        Cart GetActiveCartOfUser(int userid);
        Cart GetCart(int cartid);
        List<Cart> GetAllPreviousCartsOfUser(int userid);
	}
}

