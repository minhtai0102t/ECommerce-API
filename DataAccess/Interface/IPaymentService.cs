using System;
using ECommerce.API.Models;

namespace ECommerce.API.DataAccess
{
	public interface IPaymentService
	{
        List<Payment> GetPayments();
        Payment GetPaymentById(int id);
        bool InsertPayment(InsertPaymentReq payment);
        bool UpdatePayment(int id, UpdatePaymentReq payment);
        bool DeletePayment(int id);
    }
}

