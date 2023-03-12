using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IPaymentService
	{
        List<PaymentMethod> GetPaymentMethods();
        int InsertPayment(Payment payment);
	}
}

