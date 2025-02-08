using AspNerCoreIdentityApp.Web.Areas.Admin.Models;
using AspNerCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNerCoreIdentityApp.Web.Areas.Admin.Controllers
{
	[Authorize(Roles ="Admin")]
	[Area("Admin")]
	public class HomeController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public HomeController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> UserList()
		{
			var userlist = await _userManager.Users.ToListAsync();
			var userViewModalList = userlist.Select(x => new UserViewModel()
			{
				Id = x.Id,
				Email=x.Email,
				Name=x.UserName
			}).ToList();
			return View(userViewModalList);
		}
	}
}
