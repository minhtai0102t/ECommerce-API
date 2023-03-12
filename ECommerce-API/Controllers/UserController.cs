using System;
using ECommerce.API.DataAccess;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService dataAccess;
        private readonly string DateFormat;
        public UserController(IUserService dataAccess, IConfiguration configuration)
        {
            this.dataAccess = dataAccess;
            DateFormat = configuration["Constants:DateFormat"];
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            user.CreatedAt = DateTime.Now.ToString(DateFormat);
            user.ModifiedAt = DateTime.Now.ToString(DateFormat);

            var result = dataAccess.InsertUser(user);

            string? message;
            if (result) message = "inserted";
            else message = "email not available";
            return Ok(message);
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] User user)
        {
            var token = dataAccess.IsUserPresent(user.Email, user.Password);
            if (token == "") token = "invalid";
            return Ok(token);
        }
        [HttpGet("GetUsers")]
        public IActionResult GetAllUser()
        {
            var result = dataAccess.GetAllUser();
            return Ok(result);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(User id)
        {
            var delete = dataAccess.Delete(id);
            return Ok(delete);
        }
        [HttpPut("Update")]
        public IActionResult Update(User id)
        {
            var update = dataAccess.Update(id);
            return Ok(update);
        }
    }
}

