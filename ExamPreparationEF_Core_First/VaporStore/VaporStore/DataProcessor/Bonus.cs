using System.Linq;
using System.Text;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor
{
	using System;
	using Data;

	public static class Bonus
	{
		public static string UpdateEmail(VaporStoreDbContext context, string username, string newEmail)
		{

		    var user = context.Users.SingleOrDefault(x => x.Username == username);

		    if (user == null)
		    {
		        return ($"User {username} not found");
		    }

		    var email = context.Users.Any(x => x.Email == newEmail);

		    if (email)
		    {
		        return ($"Email {newEmail} is already taken");
		    }

		    user.Email = newEmail;
		  
		    context.SaveChanges();

		    return $"Changed {username}'s email successfully";

		}
	}
}
