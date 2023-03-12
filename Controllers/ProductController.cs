using System;
using ECommerce.API.DataAccess;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDataAccess dataAccess;
        private readonly IConfiguration configuration;
        public ProductController(IDataAccess IdataAccess, IConfiguration Iconfiguration)
		{
            dataAccess = IdataAccess;
            configuration = Iconfiguration;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts(string category, string subcategory, int count)
        {
            var result = dataAccess.GetProducts(category, subcategory, count);
            return Ok(result);
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var result = dataAccess.GetProduct(id);
            return Ok(result);
        }
        [HttpPut("UpdateProduct/{id}")]
        public IActionResult UpdateProduct(int id)
        {
            var result = dataAccess.UpdateProduct(id);
            return Ok(result);
        }
    }
}

