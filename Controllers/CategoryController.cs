using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
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
        [HttpPost("InsertProductCategory")]
        public IActionResult InsertCategory(ProductCategory category)
        {
            var result = dataAccess.InsertCategory(category);
            return Ok(result ? "inserted" : "insert fail");
        }
        [HttpPut("UpdateCategory/id")]
        public IActionResult UpdateCategory(ProductCategory id)
        {
            var result = dataAccess.UpdateCategory(id);
            return Ok(result ? "updated" : "update fail");
        }
        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            var result = dataAccess.DeleteCategory(id);
            return Ok(result ? "deleted" : "delete fail");
        }
    }
}

