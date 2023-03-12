using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IOrderService
	{
		public int InsertOrder(Order order);
	}
}

