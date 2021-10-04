using ILoveBaku.Application.Common.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _option;
        public EmailSender(IOptions<EmailSenderOptions> option)
        {
            _option = option.Value;
        }
        public async Task SendEmailAsync(string toEmail, string Subject, string Message)
        {
            using (MailMessage msg = new MailMessage())
            {
                using (SmtpClient connect = new SmtpClient(_option.Host, _option.Port))
                {
                    msg.From = new MailAddress(_option.Email, _option.DisplayName, System.Text.Encoding.UTF8);
                    msg.To.Add(toEmail);
                    msg.Subject = Subject;
                    msg.Body = Message;
                    msg.IsBodyHtml = true;

                    connect.UseDefaultCredentials = true;
                    connect.Credentials = new NetworkCredential(_option.Email, _option.Password);
                    connect.EnableSsl = _option.EnableSSL;
                    await connect.SendMailAsync(msg);
                }
            }
        }
    }

  
    public class EmailSenderOptions
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}
