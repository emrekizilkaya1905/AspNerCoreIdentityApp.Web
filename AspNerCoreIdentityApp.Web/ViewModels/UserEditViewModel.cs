using AspNerCoreIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.ViewModels
{
	public class UserEditViewModel
	{
		[Required(ErrorMessage = "Kullanici ad alani bos birakilamaz.")]
		[Display(Name = "Kullanici Adi :")]
		public string UserName { get; set; } = null!;
		[EmailAddress(ErrorMessage = "Email formati yanlis.")]
		[Display(Name = "Email :")]
		[Required(ErrorMessage = "Email alani bos birakilamaz.")]
		public string Email { get; set; } = null!;
		[Display(Name = "Telefon :")]
		[Required(ErrorMessage = "Telefon alani bos birakilamaz.")]
		public string Phone { get; set; } = null!;

		[Display(Name = "Dogum Tarihi :")]
		[DataType(DataType.Date)]
		public DateTime? Birthdate { get; set; }
		[Display(Name = "Sehir:")]

		public string? City { get; set; }
		[Display(Name = "Resim:")]

		public IFormFile? Picture { get; set; }
		[Display(Name = "Cinsiyet:")]

		public Gender? Gender { get; set; }
	}
}
