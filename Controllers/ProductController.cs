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
    public class ProductController : ControllerBase
    {
        private readonly IProductService dataAccess;
        private readonly string DateFormat;
        public ProductController(IProductService dataAccess, IConfiguration configuration)
		{
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }
        [HttpGet("GetProducts")]
        public IActionResult GetProducts(string category, string subcategory, int count)
        {
            var result = dataAccess.GetProducts(category, subcategory, count);
            return Ok(result);
        }
        [HttpGet("GetProductsByQuantity/{count}")]
        public IActionResult GetProductsByQuantity(int count)
        {
            var result = dataAccess.GetProductsByQuantity(count);
            return Ok(result);
        }
        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var result = dataAccess.GetProduct(id);
            return Ok(result);
        }
        [HttpPost("InsertProduct")]
        public IActionResult InsertProduct(UpdateProductReq req)
        {
            var result = dataAccess.InsertProduct(req);
            string? message;
            if (result) message = "inserted";
            else message = "fail";
            return Ok(message);
        }
        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(UpdateProductReq req)
        {
            var result = dataAccess.UpdateProduct(req);
            string? message;
            if (result) message = "updated";
            else message = "fail";
            return Ok(message);
        }
        [HttpDelete("DelteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var result = dataAccess.DeleteProduct(id);
            string? message;
            if (result) message = "deleted";
            else message = "fail";
            return Ok(message);
        }
    }
}

