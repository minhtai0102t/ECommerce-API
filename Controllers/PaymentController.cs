using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
	{
        private readonly IPaymentService dataAccess;
        private readonly string DateFormat;
        public PaymentController(IPaymentService dataAccess, IConfiguration configuration)
		{
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }
        [HttpGet("GetPayments")]
        public IActionResult GetPayments()
        {
            var result = dataAccess.GetPayments();
            return Ok(result);
        }
        [HttpGet("GetPaymentById")]
        public IActionResult GetPaymentById(int id)
        {
            var result = dataAccess.GetPaymentById(id);
            return Ok(result);
        }
        [HttpPost("InsertPayment")]
        public IActionResult InsertPayment(InsertPaymentReq payment)    
        {
            payment.CreatedAt = DateTime.Now.ToString();
            var result = dataAccess.InsertPayment(payment);
            return Ok(result ? "inserted" : "insert fail");
        }
        [HttpPut("UpdatePayment")]
        public IActionResult UpdatePayment(int id, UpdatePaymentReq payment)
        {
            payment.CreatedAt = DateTime.Now.ToString();
            var result = dataAccess.UpdatePayment(id, payment);
            return Ok(result ? "updated" : "update fail");
        }
        [HttpDelete("DeletePayment")]
        public IActionResult DeletePayment(int id) {
            var result = dataAccess.DeletePayment(id);
            return Ok(result ? "deleted" : "delete fail");
        }
    }
}

