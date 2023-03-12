using System;
using ECommerce.API.DataAccess;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService dataAccess;
        private readonly string DateFormat;
        public CategoryController(ICategoryService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }

        [HttpGet("GetCategoryList")]
        public IActionResult GetCategoryList()
        {
            var result = dataAccess.GetProductCategories();
            return Ok(result);
        }

        [HttpGet("GetProductCategory/id")]
        public IActionResult GetProductCategory(int id)
        {
            var result = dataAccess.GetProductCategory(id);
            return Ok(result);
        }
    }
}

