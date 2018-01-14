using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using System;
using System.Web.Script.Serialization;
using SendGrid;
using SendGrid.Helpers.Mail; // Include if you want to use the Mail Helper
using EF.Data;

namespace DataService.Services
{
    public class EmailService : IIdentityMessageService
    {
        IEmailRepository _emailRepo;
        public EmailService(IEmailRepository emailRepo)
        {
            _emailRepo = emailRepo;
        }
        public async Task SendAsync(IdentityMessage message)
        {
            await SendEmailAsync(message);
            var emailHistory = new EmailHistory()
            {
                Body = message.Body,
                ToEmailAddress = message.Destination,
                Subject = message.Subject,
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now
            };
            _emailRepo.Save(emailHistory);
        }

        // Use NuGet to install SendGrid (Basic C# client lib)
        private async Task SendEmailAsync(IdentityMessage message)
        {
            string apiKey = ConfigurationManager.AppSettings["emailService:SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@TradesMate.com", "TradesMate Admin");
            var subject = message.Subject;
            EmailAddress to = null;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["emailService:CompulsoryEmail"]))
            {
                 to = new EmailAddress(ConfigurationManager.AppSettings["emailService:CompulsoryEmail"]);
            }
            else
            {
                 to = new EmailAddress(message.Destination);
            }
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}