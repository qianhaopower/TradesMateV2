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
            //var myMessage = new SendGridMessage();

            //myMessage.AddTo(message.Destination);
            //myMessage.From = new System.Net.Mail.MailAddress("noreply@TradesMate.com", "TradesMate");
            //myMessage.Subject = message.Subject;
            //myMessage.Text = message.Body;
            //myMessage.Html = message.Body;

            //var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
            //                                        ConfigurationManager.AppSettings["emailService:Password"]);

            //// Create a Web transport for sending email.
            //var transportWeb = new Web(credentials);

            //var myMessage = new SendGrid.SendGridMessage();
            //myMessage.AddTo("test@sendgrid.com");
            //myMessage.From = new MailAddress("you@youremail.com", "First Last");
            //myMessage.Subject = "Sending with SendGrid is Fun";
            //myMessage.Text = "and easy to do anywhere, even with C#";

            //var transportWeb = new SendGrid.Web("SENDGRID_APIKEY");
            //transportWeb.DeliverAsync(myMessage);



            string apiKey = ConfigurationManager.AppSettings["emailService:SendGridApiKey"];
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email("noreply@TradesMate.com");
            string subject = message.Subject;
            Email to = new Email(message.Destination);
            Content content = new Content("text/html", message.Body);
            Mail mail = new Mail(from, subject, to, content);


           


            // do not sent the email for now
            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());





            // Send the email.
            //if (transportWeb != null)
            //{
            //    await transportWeb.DeliverAsync(myMessage);
            //}
            //else
            //{
            //    //Trace.TraceError("Failed to create Web transport.");
            //    await Task.FromResult(0);
            //}
        }
    }
}