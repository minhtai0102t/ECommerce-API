using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IOrderService dataAccess;
        private readonly string DateFormat;
        public OrderController(IOrderService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }
        [HttpGet("GetOrders")]
        public IActionResult GetOrders()
        {
            var result = dataAccess.GetOrders();
            return Ok(result);
        }
        [HttpGet("GetOrderById/{id}")]
        public IActionResult GetOrderById(int id)
        {
            var result = dataAccess.GetOrderById(id);
            return Ok(result);
        }
        [HttpPost("InsertOrder")]
        public IActionResult InsertOrder(InsertOrderReq order)
        {
            order.CreatedAt = DateTime.Now.ToString();
            var id = dataAccess.InsertOrder(order);
            return Ok(id.ToString());
        }
        [HttpDelete("DeleteOrderById/{id}")]
        public IActionResult DeleteOrderById(int id)
        {
            var result = dataAccess.DeleteOrder(id);
            return Ok(result ? "deleted" : "delete fail");
        }
    }
}
