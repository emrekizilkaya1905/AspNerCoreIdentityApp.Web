﻿namespace AspNerCoreIdentityApp.Web.Services
{
	public interface IEmailService
	{
		Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);
	}
}
