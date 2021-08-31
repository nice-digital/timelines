using System;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using NICE.Timelines.Configuration;

namespace NICE.Timelines.Services
{
    public interface IEmailService
    {
        void SendEmail(string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILogger<EmailService> _logger;

        public EmailService(EmailConfig emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }

        public void SendEmail(string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_emailConfig.Server, _emailConfig.Port, false);
                if (!string.IsNullOrEmpty(_emailConfig.Username) && !string.IsNullOrEmpty(_emailConfig.Password))
                {
                    client.Authenticate(_emailConfig.Username, _emailConfig.Password);
                }

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_emailConfig.SenderAddress));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;
                message.Body = bodyBuilder.ToMessageBody();

                foreach (var emailAddress in _emailConfig.RecipientAddresses)
                {
                    message.To.Clear();
                    message.To.Add(MailboxAddress.Parse(emailAddress));

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error sending email: {e}");
                    }
                }

                client.Disconnect(true);
            }
        }
    }
}
