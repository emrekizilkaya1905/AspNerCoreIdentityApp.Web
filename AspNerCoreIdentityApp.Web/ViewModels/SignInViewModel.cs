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
		public string Email { get; set; } = null!;

		[Display(Name = "Sifre :")]
		[Required(ErrorMessage = "Sifre alani bos birakilamaz.")]
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Sifreniz en az 6 karakteri olmalidir.")]
		public string Password { get; set; } = null!;

		[Display(Name = "Beni Hatirla ")]
		public bool RememberMe { get; set; } 
	}
}

