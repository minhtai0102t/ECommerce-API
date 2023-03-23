using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using ECommerce.API.Models.Request;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
	{
        private readonly ICartService dataAccess;
        private readonly string DateFormat;
		public CartController(ICartService dataAccess, IConfiguration configuration)
		{
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
		}

        [HttpPost("InsertCartItem/{userid}/{productid}/{quantity}")]
        public IActionResult InsertCartItem(int userid, int productid, int quantity)
        {
            var result = dataAccess.InsertCartItem(userid, productid, quantity);
            return Ok(result ? "inserted" : "insert fail");
        }

        [HttpGet("GetActiveCartOfUser/{id}")]
        public IActionResult GetActiveCartOfUser(int id)
        {
            var result = dataAccess.GetActiveCartOfUser(id);
            return Ok(result);
        }

        [HttpGet("GetAllPreviousCartsOfUser/{id}")]
        public IActionResult GetAllPreviousCartsOfUser(int id)
        {
            var result = dataAccess.GetAllPreviousCartsOfUser(id);
            return Ok(result);
        }
        [HttpPut("UpdateCartItemQuantity/{userid}/{productid}/{quantity}")]
        public IActionResult UpdateCartItemQuantity(int userid, int productid, int quantity)
        {
            var result = dataAccess.UpdateCartItemQuantity(userid, productid, quantity);
            return Ok(result ? "updated" : "update fail");
        }
        [HttpDelete("DeleteCartItem/{userid}/{productid}/{quantity}")]
        public IActionResult DeleteCartItem(int userid, int productid, int quantity)
        {
            var result = dataAccess.DeleteCartItem(userid, productid, quantity);
            return Ok(result ? "deleted" : "delete fail");
        }
        [HttpDelete("DeleteAllCartItem/{userid}")]
        public IActionResult DeleteAllCartItem(int userid)
        {
            var result = dataAccess.DeleteAllCartItem(userid);
            return Ok(result ? "deleted" : "delete fail");
        }
        [HttpGet("TotalOfCarts")]
        public IActionResult TotalOfCarts()
        {
            var result = dataAccess.TotalOfCarts();
            return Ok(result);
        }
    }
}

