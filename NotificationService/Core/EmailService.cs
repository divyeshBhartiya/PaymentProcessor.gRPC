using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace NotificationService.Core
{
    public class EmailService : IEmailService
    {
        private NotificationMetadata _notificationMetadata;
        public EmailService(NotificationMetadata notificationMetadata)
        {
            _notificationMetadata = notificationMetadata;
        }

        public async Task<Response> SendAsync(string from, RepeatedField<string> to, string subject, string html)
        {
            try
            {
                foreach (var reciever in to)
                {
                    EmailMessage message = new EmailMessage
                    {
                        Sender = new MailboxAddress("Payment Update", from),
                        Reciever = new MailboxAddress(string.Empty, reciever),
                        Subject = subject,
                        Content = html
                    };
                    var mimeMessage = CreateMimeMessageFromEmailMessage(message);

                    using SmtpClient smtpClient = new SmtpClient();
                    await smtpClient.ConnectAsync(_notificationMetadata.SmtpServer, _notificationMetadata.Port, true);
                    smtpClient.Authenticate(_notificationMetadata.UserName, _notificationMetadata.Password);
                    await smtpClient.SendAsync(mimeMessage);
                    smtpClient.Disconnect(true);
                }
                Console.WriteLine("Emails are sent successfully. Relax!!!");
                return new Response { Message = "Emails sent successfully", Status = Status.Success };
            }
            catch(Exception ex)
            {
                return new Response { Message = ex.StackTrace, Status = Status.Fail };
            }
        }

        private static MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Flowed)
            { Text = message.Content };
            return mimeMessage;
        }
    }
}
