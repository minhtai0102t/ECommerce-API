using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IPaymentService
	{
        int InsertPayment(Payment payment);
	}
}

