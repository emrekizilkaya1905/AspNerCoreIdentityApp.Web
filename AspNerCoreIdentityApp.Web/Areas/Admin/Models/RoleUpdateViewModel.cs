using System.ComponentModel.DataAnnotations;

namespace AspNerCoreIdentityApp.Web.Areas.Admin.Models
{
	public class RoleUpdateViewModel
	{
		public string Id { get; set; } = null!;
		[Required(ErrorMessage = "Rol isim alani bos birakilamaz.")]
		[Display(Name = "Role ismi")]
		public string? Name { get; set; }
	}
}
