using System;
using ECommerce.API.Models;
namespace ECommerce.API.DataAccess
{
	public interface IUserService
	{
		bool InsertUser(User user);
        string IsUserPresent(string email, string password);
        User GetUser(int id);
	}
}

