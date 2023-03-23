using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IOrderService
	{
        public List<Order> GetOrders();
        public Order GetOrderById(int id);
        public int InsertOrder(InsertOrderReq order);
		public bool DeleteOrder(int id);

    }
}

