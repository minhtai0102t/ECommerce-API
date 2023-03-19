using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IPaymentMethodService : IDisposable
	{
        List<PaymentMethod> GetPaymentMethods();
        PaymentMethod GetPaymentMethodById(int id);
        bool InsertPaymentMethod(PaymentMethod method);
        bool UpdatePaymentMethod(PaymentMethod method);
        bool DeletePaymentMethod(int id);
    }
}

