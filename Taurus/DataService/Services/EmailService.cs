using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using System;
using System.Web.Script.Serialization;
using SendGrid;
using SendGrid.Helpers.Mail; // Include if you want to use the Mail Helper


namespace DataService.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib)
        private async Task configSendGridasync(IdentityMessage message)
        {
            string apiKey = ConfigurationManager.AppSettings["emailService:SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@TradesMate.com", "Example User");
            var subject = message.Subject;
            var to = new EmailAddress(message.Destination);
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}