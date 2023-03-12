using System;
using ECommerce.API.DataAccess;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
	{
        private readonly IDataAccess _dataAccess;
        private readonly IConfiguration _configuration;
		public CartController(IDataAccess dataAccess, IConfiguration configuration)
		{
            _dataAccess = dataAccess;
            _configuration = configuration;
		}

        [HttpPost("InsertCartItem/{userid}/{productid}")]
        public IActionResult InsertCartItem(int userid, int productid)
        {
            var result = _dataAccess.InsertCartItem(userid, productid);
            return Ok(result ? "inserted" : "not inserted");
        }

        [HttpGet("GetActiveCartOfUser/{id}")]
        public IActionResult GetActiveCartOfUser(int id)
        {
            var result = _dataAccess.GetActiveCartOfUser(id);
            return Ok(result);
        }

        [HttpGet("GetAllPreviousCartsOfUser/{id}")]
        public IActionResult GetAllPreviousCartsOfUser(int id)
        {
            var result = _dataAccess.GetAllPreviousCartsOfUser(id);
            return Ok(result);
        }
    }
}

