using HC.Common.Model.OrganizationSMTP;
using HC.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HC.Common.Services
{

    public class EmailService : IEmailService
    {
        private readonly EmailConfig ec;        

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            this.ec = emailConfig.Value;

        }

        /// <summary>
        /// email send method
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(String email, String subject, String message, OrganizationSMTPCommonModel organizationSMTPModel, string organizationName)
        {
            try
            {
                var emailMessage = new MimeMessage();
                this.ec.FromAddress = organizationSMTPModel.SMTPUserName; //"smartDataHC@gmail.com";
                this.ec.FromName = organizationName.ToString();
                this.ec.MailServerAddress = organizationSMTPModel.ServerName; //"smtp.gmail.com";
                this.ec.UserPassword = organizationSMTPModel.SMTPPassword; //"hccare123*";
                this.ec.MailServerPort = organizationSMTPModel.Port; //"587";
                this.ec.UserId = organizationSMTPModel.SMTPUserName; //"smartDataHC@gmail.com";
                emailMessage.From.Clear();
                emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));

                emailMessage.To.Clear();
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = ec.LocalDomain;

                    await client.ConnectAsync(ec.MailServerAddress, Convert.ToInt32(ec.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
