﻿using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.ViewModels
{
	public class SignUpViewModel
	{
		public SignUpViewModel()
		{
			
		}
		public SignUpViewModel(string userName, string email, string phone, string password)
		{
			UserName = userName;
			Email = email;
			Phone = phone;
			Password = password;
		}
		[Required(ErrorMessage = "Kullanici ad alani bos birakilamaz.")]
		[Display(Name = "Kullanici Adi :")]
		public string UserName { get; set; } = null!;
		[EmailAddress(ErrorMessage ="Email formati yanlis.")]
		[Display(Name = "Email :")]
		[Required(ErrorMessage = "Email alani bos birakilamaz.")]
		public string Email { get; set; } = null!;
		[Display(Name = "Telefon :")]
		[Required(ErrorMessage = "Telefon alani bos birakilamaz.")]
		public string Phone { get; set; } = null!;
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Sifreniz en az 6 karakteri olmalidir.")]
		[Display(Name = "Sifre :")]
		[Required(ErrorMessage = "Sifre alani bos birakilamaz.")]
		public string Password { get; set; } = null!;
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Sifreniz en az 6 karakteri olmalidir.")]
		[Display(Name = "Sifre Tekrari :")]
		[Required(ErrorMessage = "Sifre tekrar alani bos birakilamaz.")]
		[Compare(nameof(Password),ErrorMessage ="Sifre ayni degildir.")]
		public string PasswordConfirm { get; set; } = null!; 
	}
}
