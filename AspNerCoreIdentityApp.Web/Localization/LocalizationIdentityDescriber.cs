using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AspNerCoreIdentityApp.Web.Localization
{
	public class LocalizationIdentityDescriber:IdentityErrorDescriber
	{
		public override IdentityError DuplicateUserName(string userName)
		{
			return new()
			{
				Code = "DuplicateUserName",
				Description = $"{userName} baska bir kullanici tarafindan alinmistir."
			};
			//return base.DuplicateUserName(userName);
		}
		public override IdentityError DuplicateEmail(string email)
		{
			return new()
			{
				Code = "DuplicateEmail",
				Description = $"{email} baska bir kullanici tarafindan alinmistir."
			};
		}
		public override IdentityError PasswordTooShort(int length)
		{
			return new()
			{
				Code = "TooShortPassword",
				Description = $"{length} password en az 6 karakter almalidir."
			};
		}
	}
}
