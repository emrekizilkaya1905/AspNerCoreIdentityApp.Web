﻿using Microsoft.AspNetCore.Identity;

namespace AspNerCoreIdentityApp.Web.Models
{
	public class AppUser:IdentityUser
	{
		public string? City { get; set; }
		public string? Picture { get; set; }
		public DateTime? Birthdate { get; set; }
		public Gender? Gender { get; set; }	
	}
}
