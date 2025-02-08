using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.ViewModels
{
	public class PasswordChangeViewModel
	{
		[DataType(DataType.Password)]
		[Display(Name = "Sifre :")]
		[Required(ErrorMessage = "Sifre alani bos birakilamaz.")]
		[MinLength(6, ErrorMessage = "Sifreniz en az 6 karakteri olmalidir.")]
		public string? PasswordOld { get; set; } = null!;
		[DataType(DataType.Password)]
		[Display(Name = "Yeni Sifre :")]
		[MinLength(6, ErrorMessage = "Sifreniz en az 6 karakteri olmalidir.")]
		[Required(ErrorMessage = "Yeni sifre alani bos birakilamaz.")]
		public string? PasswordNew { get; set; } = null!;
		[DataType(DataType.Password)]
		[Display(Name = " Yeni Sifre Tekrari :")]
		[MinLength(6, ErrorMessage = "Sifreniz en az 6 karakteri olmalidir.")]
		[Required(ErrorMessage = "Yeni sifre tekrar alani bos birakilamaz.")]
		[Compare(nameof(PasswordNew), ErrorMessage = "Sifre ayni degildir.")]
		public string? PasswordNewConfirm { get; set; } = null!;
	}
}
