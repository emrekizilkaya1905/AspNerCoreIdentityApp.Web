using System.Diagnostics;
using AspNerCoreIdentityApp.Web.Models;
using AspNerCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNerCoreIdentityApp.Web.Extensions;
using AspNerCoreIdentityApp.Web.Services;
using System.Security.Claims;

namespace AspNerCoreIdentityApp.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private readonly UserManager<AppUser> _UserManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IEmailService _emailService;
		public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, IEmailService emailservice)
		{
			_logger = logger;
			_UserManager = userManager;
			_signInManager = signInManager;
			_emailService = emailservice;
			;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult SignUp()
		{

			return View();
		}
		public IActionResult SignIn()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			returnUrl ??= Url.Action("Index", "Home");
			var hasUser = await _UserManager.FindByEmailAsync(model.Email);
			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Email veya sifre yanlis.");
				return View();
			}

			var signinResult = await _signInManager.PasswordSignInAsync
			(hasUser, model.Password, model.RememberMe, true);

			if (signinResult.IsLockedOut)
			{
				ModelState.AddModelErrorList(new List<string>()
				{ "3 dakika boyunca giris yapamazsiniz." });
				return View();
			}
			if (!signinResult.Succeeded)
			{
				ModelState.AddModelErrorList(new List<string>()
			{ @$"Email veya sifre yanlis.",$"Basarisiz giris sayisi =" +
			$"{ await _UserManager.GetAccessFailedCountAsync(hasUser)}" });
				return View();
			}
			if (hasUser.Birthdate.HasValue)
			{
				await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe,
						new[] { new Claim("birthdate", hasUser.Birthdate.Value.ToString()) });

			}
			return Redirect(returnUrl!);




		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel request)
		{

			if (!ModelState.IsValid)
			{
				return View();
			}
			var identityResult = await _UserManager.CreateAsync(new AppUser()
			{
				UserName = request.UserName,
				PhoneNumber = request.Phone,
				Email = request.Email
			}, request.PasswordConfirm);
			if (!identityResult.Succeeded)
			{
				ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
				return View();
			}


			var exchangeExxpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
			var user = await _UserManager.FindByNameAsync(request.UserName);
			var claimResult = await _UserManager.AddClaimAsync(user!, exchangeExxpireClaim);
			if (!claimResult.Succeeded)
			{
				ModelState.AddModelErrorList(claimResult.Errors.Select(x => x.Description).ToList());
				return View();
			}
			TempData["SuccessMessage"] = "Uyelik islemi basarili ile gerceklesmistir";
			return RedirectToAction(nameof(HomeController.SignUp));
		}

		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
		{
			var hasUser = await _UserManager.FindByEmailAsync(request.Email!);
			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanici bulunamamistir.");
				return View();
			}
			string passwordResetToken = await _UserManager.GeneratePasswordResetTokenAsync(hasUser);
			var resetPasLink = Url.Action("ResetPassword", "Home",
			  new { userId = hasUser.Id, Token = passwordResetToken },
			  HttpContext.Request.Scheme);


			await _emailService.SendResetPasswordEmail(resetPasLink!, hasUser.Email!);
			TempData["SuccessMessage"] = "Sifre yenileme linki adresinize gonderilmistir.";
			return RedirectToAction(nameof(ForgetPassword));
		}

		public IActionResult ResetPassword(string userId, string token)
		{
			TempData["userId"] = userId;
			TempData["token"] = token;

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
		{
			var userId = TempData["userId"];
			var token = TempData["token"];
			if (userId == null || token == null)
			{
				throw new Exception("Bir hata meydana geldi.");
			}

			var hasUser = await _UserManager.FindByIdAsync(userId.ToString()!);
			if (hasUser == null)
			{
				ModelState.AddModelError(string.Empty, "Kullanici bulunamamistir");
				return View();
			}
			var result = await _UserManager.ResetPasswordAsync(hasUser,
			token.ToString()!, request.Password!);
			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = "Sifreniz basariyla yenilenmistir";

			}
			else
			{
				ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());

			}
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
