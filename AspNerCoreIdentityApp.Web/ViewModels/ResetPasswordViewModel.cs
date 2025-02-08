using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.ViewModels
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]
		[Display(Name = "Yeni Sifre :")]
		[Required(ErrorMessage = "Sifre alani bos birakilamaz.")]

		public string? Password { get; set; }
		[DataType(DataType.Password)]

		[Display(Name = "Yeni Sifre Tekrari :")]
		[Required(ErrorMessage = "Sifre tekrar alani bos birakilamaz.")]
		[Compare(nameof(Password), ErrorMessage = "Sifre ayni degildir.")]
		public string? PasswordConfirm { get; set; }
	}
}
