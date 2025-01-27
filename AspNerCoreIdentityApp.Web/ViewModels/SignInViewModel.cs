using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.ViewModels
{
	
		public class SignInViewModel // Adı düzelttik
		{
			public SignInViewModel()
			{
			}

			public SignInViewModel(string email, string password)
			{
				Email = email;
				Password = password;
			}

			[Display(Name = "Email :")]
			[Required(ErrorMessage = "Email alani bos birakilamaz.")]
			[EmailAddress(ErrorMessage = "Email formati yanlis.")]
			public string Email { get; set; }

			[Display(Name = "Sifre :")]
			[Required(ErrorMessage = "Sifre alani bos birakilamaz.")]
			public string Password { get; set; }

			[Display(Name = "Beni Hatirla :")]
			public bool RememberMe { get; set; }
		}
	}

