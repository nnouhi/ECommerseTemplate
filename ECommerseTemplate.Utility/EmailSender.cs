using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ECommerseTemplate.Utility
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _config;

		public EmailSender(IConfiguration config)
		{
			_config = config;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			string senderEmail = _config.GetValue<string>("Outlook:Email");
			string senderPassword = _config.GetValue<string>("Outlook:Password");

			var client = new SmtpClient("smtp.office365.com", 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(senderEmail, senderPassword)
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(senderEmail),
				Subject = subject,
				Body = htmlMessage,
				IsBodyHtml = true
			};

			mailMessage.To.Add(email);

			return client.SendMailAsync(mailMessage);
		}
	}
}
