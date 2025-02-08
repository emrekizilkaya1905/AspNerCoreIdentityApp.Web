using AspNerCoreIdentityApp.Web.Areas.Admin.Models;
using AspNerCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNerCoreIdentityApp.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AspNerCoreIdentityApp.Web.Areas.Admin.Controllers
{

	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class RolesController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;

		public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		[Authorize(Roles = "role-action")]
		public async Task<IActionResult> Index()
		{
			var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
			{
				Id = x.Id,
				Name = x.Name!,
			}).ToListAsync();
			return View(roles);
		}
		[Authorize(Roles = "role-action")]
		public IActionResult RoleCreate()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "role-action")]
		public async Task<ActionResult> RoleCreate(RoleCreateViewModel request)
		{
			var result = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });
			if (!result.Succeeded)
			{
				ModelState.AddModelErrorList(result.Errors);
				return View();
			}
			TempData["SuccessMessage"] = "Rol olusturuldu.";
			return RedirectToAction(nameof(RolesController.Index));
		}
		[Authorize(Roles = "role-action")]
		public async Task<IActionResult> RoleUpdate(string id)
		{
			var roleToUpdate = await _roleManager.FindByIdAsync(id);
			if (roleToUpdate == null)
			{
				throw new Exception("Guncellenecek rol bulanamamistir.");
			}
			return View(new RoleUpdateViewModel() { Id = roleToUpdate.Id, Name = roleToUpdate!.Name! });
		}
		[HttpPost]
		[Authorize(Roles = "role-action")]
		public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
		{
			var roleToUpdate = await _roleManager.FindByIdAsync(request.Id);
			if (roleToUpdate == null)
			{
				throw new Exception("Guncellenecek rol bulanamamistir.");
			}
			roleToUpdate.Name = request.Name;
			await _roleManager.UpdateAsync(roleToUpdate);
			ViewData["SuccessMessage"] = "Rol bilgisi guncellenmistir";
			return View();
		}
		[Authorize(Roles = "role-action")]
		public async Task<IActionResult> RoleToDelete(string id)
		{
			var roleToDelete = await _roleManager.FindByIdAsync(id);
			if (roleToDelete == null)
			{
				throw new Exception("Silinecek rol bulunamamistir");
			}
			var result = await _roleManager.DeleteAsync(roleToDelete);
			if (!result.Succeeded)
			{
				throw new Exception(result.Errors.Select(x => x.Description).First());
			}
			TempData["SuccessMessage"] = "Rol silinmistir";
			return RedirectToAction(nameof(HomeController.Index));
		}
		public async Task<IActionResult> AssignRoleToUser(string id)
		{
			var currentUser = (await _userManager.FindByIdAsync(id))!;
			ViewBag.userId = id;
			var roles = await _roleManager.Roles.ToListAsync();
			var roleViewModelList = new List<AssignRoleToUserViewModel>();
			var userRoles = await _userManager.GetRolesAsync(currentUser);
			foreach (var role in roles)
			{
				var assignRoleViewModel = new AssignRoleToUserViewModel()
				{
					Id = role.Id,
					Name = role.Name!,
				};
				if (userRoles.Contains(role.Name!))
				{
					assignRoleViewModel.Exists = true;
				}
				roleViewModelList.Add(assignRoleViewModel);
			}
			return View(roleViewModelList);
		}
		[HttpPost]
		public async Task<IActionResult> AssignRoleToUser(string id, List<AssignRoleToUserViewModel> requestList)
		{
			var userToAssignRole = (await _userManager.FindByIdAsync(id))!;
			foreach (var role in requestList)
			{
				if(role.Exists)
				{
					await _userManager.AddToRoleAsync(userToAssignRole,role.Name);
				}
				else
				{
					await _userManager.RemoveFromRoleAsync(userToAssignRole,role.Name);
				}
			}
			return RedirectToAction(nameof(HomeController.UserList),"Home");
		}
	}
}
