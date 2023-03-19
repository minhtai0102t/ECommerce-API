using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService dataAccess;
        private readonly string DateFormat;
        public PaymentMethodController(IPaymentMethodService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }
        [HttpGet("GetPaymentMethods")]
        public IActionResult GetPaymentMethods()
        {
            var result = dataAccess.GetPaymentMethods();
            return Ok(result);
        }
        [HttpGet("GetPaymentMethodById{id}")]
        public IActionResult GetPaymentMethodById(int id)
        {
            var result = dataAccess.GetPaymentMethodById(id);
            return Ok(result);
        }
        [HttpPost("InsertPaymentMethod")]
        public IActionResult InsertPaymentMethod(PaymentMethod method)
        {
            var result = dataAccess.InsertPaymentMethod(method);
            return Ok(result ? "inserted" : "insert fail");
        }
        [HttpPut("UpdatePaymentMethod")]
        public IActionResult UpdatePaymentMethod(PaymentMethod method)
        {
            var result = dataAccess.UpdatePaymentMethod(method);
            return Ok(result ? "updated" : "update fail");
        }
        [HttpDelete("DeletePaymentMethod")]
        public IActionResult DeletePaymentMethod(int id)
        {
            var result = dataAccess.DeletePaymentMethod(id);
            return Ok(result ? "deleted" : "delete fail");
        }
    }
}

