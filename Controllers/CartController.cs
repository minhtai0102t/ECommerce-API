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
            return Ok(result ? "inserted" : "not inserted");
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
    }
}

