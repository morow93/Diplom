using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace InoDrive.Api.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SendEmail(message.Destination, message.Subject, message.Body);
            return Task.FromResult(0);
        }

        private void SendEmail(string email, string subject, string body)
        {
            //ned set - host, port, email, password

            var client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = WebConfigurationManager.AppSettings["host"];
            client.Port = Int32.Parse(WebConfigurationManager.AppSettings["port"]);

            var originEmail = WebConfigurationManager.AppSettings["email"];
            var originPassword = WebConfigurationManager.AppSettings["password"];

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(originEmail, originPassword);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(originEmail);
            msg.To.Add(new MailAddress(email));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            try
            {
                client.Send(msg);
            }
            catch (Exception error)//for timeout
            {
                var tmpError = error;
            }
        }
    }
}