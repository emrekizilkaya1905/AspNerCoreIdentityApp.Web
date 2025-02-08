
using AspNerCoreIdentityApp.Web.Extensions;
using AspNerCoreIdentityApp.Web.Models;
using AspNerCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Runtime.ExceptionServices;
using System.Security.Claims;

namespace AspNerCoreIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IFileProvider _fileProvider;

		public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_fileProvider = fileProvider;
		}
		public async Task<IActionResult> Index()
		{
			
			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
			var userViewModel = new UserViewModel
			{
				Email = currentUser!.Email,
				Username = currentUser!.UserName,
				Phone = currentUser!.PhoneNumber,
				PictureUrl=currentUser!.Picture
			};
			return View(userViewModel);
		}

		public async Task Logout()
		{
			await _signInManager.SignOutAsync();

		}
		public IActionResult PasswordChange()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
			var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser!, request.PasswordOld!);
			if (!checkOldPassword)
			{
				ModelState.AddModelError(string.Empty, "Eski sifreniz yanlis");
				return View();
			}
			var resultChangePassword = await _userManager.
			ChangePasswordAsync(currentUser!, request.PasswordOld!, request.PasswordNew!);
			if (!resultChangePassword.Succeeded)
			{
				ModelState.AddModelErrorList(resultChangePassword
				.Errors);
				return View();
			}
			await _userManager.UpdateSecurityStampAsync(currentUser!);
			await _signInManager.SignOutAsync();
			await _signInManager.PasswordSignInAsync
			(currentUser!, request.PasswordNew!, false, false);
			TempData["SuccessMessage"] = "Sifreniz basarili bir sekilde degistirilmistir.";
			return View();

		}
		public async Task<IActionResult> UserEdit()
		{
			var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
			var userEditViewModel = new UserEditViewModel()
			{
				UserName = currentUser.UserName!,
				Email = currentUser.Email!,
				City = currentUser.City!,
				Phone = currentUser.PhoneNumber!,
				Birthdate = currentUser.Birthdate,
				Gender = currentUser.Gender!

			};
			ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
			return View(userEditViewModel);
		}
		[HttpPost]
		public async Task<IActionResult> UserEdit(UserEditViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
			currentUser!.UserName=request.UserName;
			currentUser.Email=request.Email;
			currentUser.City=request.City;
			currentUser.Birthdate=request.Birthdate;
			currentUser.Gender=request.Gender;
			currentUser.PhoneNumber=request.Phone;
			
			if (request.Picture != null && request.Picture.Length > 0)
			{
				var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
				var randomFile = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}"; 
				var newPicturePath = Path.Combine(wwwrootFolder!.
				First(x => x.Name == "userpictures").PhysicalPath!, randomFile);
				using var stream = new FileStream(newPicturePath, FileMode.Create);
				await request.Picture.CopyToAsync(stream);
				currentUser.Picture = randomFile;
			}
			var updateResult=await _userManager.UpdateAsync(currentUser);
			if (!updateResult.Succeeded)
			{
				ModelState.AddModelErrorList (updateResult.Errors);
				return View();	
			}
			await _userManager.UpdateSecurityStampAsync(currentUser);
			await _signInManager.SignOutAsync();
			await _signInManager.SignInAsync(currentUser,true);
			TempData["SuccessMessage"] = "Uyelik bilgileriniz basarili bir sekilde degistirilmistir.";
			var userEditViewModel = new UserEditViewModel()
			{
				UserName = currentUser.UserName!,
				Email = currentUser.Email!,
				City = currentUser.City!,
				Phone = currentUser.PhoneNumber!,
				Birthdate = currentUser.Birthdate,
				Gender = currentUser.Gender!
				
			};
			return View(userEditViewModel);
		}
		public IActionResult AccessDenied(string ReturnUrl)
		{
			string message = string.Empty;
			message = "Bu sayfayi görmeye yetkiniz yoktur. Yetki almak icin yöneticiniz ile gorusun";
			ViewBag.message=message;
			return View();
		}
		public IActionResult Claims()
		{
			var userClaimList = User.Claims.Select(x => new ClaimViewModel
			{
				Issuer = x.Issuer,
				Type = x.Type,
				Value = x.Value
			}).ToList();
			return View(userClaimList);
		}
		[Authorize(Policy = "AnkaraPolicy")]
		public IActionResult AnkaraPage()
		{

			return View();
		}
	}
}
