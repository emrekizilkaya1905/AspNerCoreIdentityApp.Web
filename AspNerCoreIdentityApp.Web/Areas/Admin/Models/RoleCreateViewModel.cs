using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.Areas.Admin.Models
{
	public class RoleCreateViewModel
	{
		[Required(ErrorMessage ="Rol ismi bos birakilamaz.")]
		[Display(Name = "Role ismi")]
		public string? Name { get; set; }
	}
}
