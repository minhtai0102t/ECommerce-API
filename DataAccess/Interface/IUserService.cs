using System;
using ECommerce.API.Models;
namespace ECommerce.API.DataAccess
{
	public interface IUserService
	{
		bool InsertUser(User user);
        string IsUserPresent(string email, string password);
		List<User> GetAllUser();
        User GetUser(int id);
		bool Update (User id);
		bool Delete(int id);
		int TotalOfUsers();
    }
}

