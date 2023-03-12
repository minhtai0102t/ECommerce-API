using System;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Business.Interface
{
	public interface IUserService : IDisposable
	{
        Task<IActionResult> RegisterUser(User obj);
	}
}

