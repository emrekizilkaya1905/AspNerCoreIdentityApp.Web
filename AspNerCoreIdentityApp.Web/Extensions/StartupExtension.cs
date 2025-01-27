using AspNerCoreIdentityApp.Web.CustomValidations;
using AspNerCoreIdentityApp.Web.Localization;
using AspNerCoreIdentityApp.Web.Models;

namespace AspNerCoreIdentityApp.Web.Extensions
{
	public static class StartupExtension
	{
		public static void AddIdentityWithExtension(this IServiceCollection services)
		{
			services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstwyzäåq0123456789_";
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = false;
				options.Password.RequireDigit = false;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
				options.Lockout.MaxFailedAccessAttempts = 3;

			}).AddPasswordValidator<PasswordValidator>()
			.AddUserValidator<UserValidator>()
			.AddErrorDescriber<LocalizationIdentityDescriber>()
			.AddEntityFrameworkStores<AppDbContext>();
		}
	}
}
