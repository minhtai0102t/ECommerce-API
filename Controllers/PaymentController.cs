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
        private readonly IDataAccess _dataAccess;
        private readonly IConfiguration _configuration;
        public PaymentController(IDataAccess dataAccess, IConfiguration configuration)
		{
            _dataAccess = dataAccess;
            _configuration = configuration;
        }

        [HttpGet("GetPaymentMethods")]
        public IActionResult GetPaymentMethods()
        {
            var result = _dataAccess.GetPaymentMethods();
            return Ok(result);
        }

        [HttpPost("InsertPayment")]
        public IActionResult InsertPayment(Payment payment)
        {
            payment.CreatedAt = DateTime.Now.ToString();
            var id = _dataAccess.InsertPayment(payment);
            return Ok(id.ToString());
        }
    }
}

