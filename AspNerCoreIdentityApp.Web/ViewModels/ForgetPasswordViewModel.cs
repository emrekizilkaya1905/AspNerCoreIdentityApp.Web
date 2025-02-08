using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[Display(Name = "Email :")]
		[Required(ErrorMessage = "Email alani bos birakilamaz.")]
		[EmailAddress(ErrorMessage = "Email formati yanlis.")]
		public string? Email { get; set; }
	}
}
