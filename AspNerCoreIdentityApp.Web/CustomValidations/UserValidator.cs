using AspNerCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace AspNerCoreIdentityApp.Web.CustomValidations
{
	public class UserValidator : IUserValidator<AppUser>
	{
		public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
		{
			var errors = new List<IdentityError>();
			var isNumeric = int.TryParse(user.UserName[0]!.ToString(), out _);
			if(isNumeric)
			{
				errors.Add(new() { Code = "UsernameContainFirstLetterDigit",
				Description="Kullanici adinin ilk kararkteri sayisal bir karakter olamaz."});
			}

			if (errors.Any())
			{
				return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
			}
			return Task.FromResult(IdentityResult.Success);
		}
	}
}
