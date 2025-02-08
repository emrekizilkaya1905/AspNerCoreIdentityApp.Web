
using AspNerCoreIdentityApp.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNerCoreIdentityApp.Web.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;
		public EmailService(IOptions<EmailSettings> options)
		{
			_emailSettings = options.Value;
		}
		public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
		{
			var smtpClient = new SmtpClient();
			smtpClient.Host =_emailSettings.Host!;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
			smtpClient.EnableSsl = true;
			var mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(_emailSettings.Email!);
			mailMessage.To.Add(ToEmail);
			mailMessage.Subject = "Localhost | Sifre sifirlama linki";
			mailMessage.Body = @$"
				<h4>Şifre Sıfırlama Linki</h4>
				<p>
				<a href='{resetPasswordEmailLink}'>Şifrenizi sıfırlamak için buraya tıklayın</a>
				</p>";
			mailMessage.IsBodyHtml = true;
			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}
