using ILoveBaku.Application.CQRS.Product.Queries.GetProductInvoice;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceOption _option;
        public EmailService(IOptions<EmailServiceOption> option)
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
                    //if (FilePath != null)
                    //{
                    //    System.Net.Mail.Attachment attachment;
                    //    attachment = new System.Net.Mail.Attachment(FilePath);
                    //    msg.Attachments.Add(attachment);
                    //    attachment.ContentId = "imgContentId";
                    //}
                    connect.Credentials = new NetworkCredential(_option.Email, _option.Password);
                    connect.EnableSsl = _option.EnableSSL;
                    await connect.SendMailAsync(msg);
                }
            }
        }

        public async Task SendInvoiceAsync(ProductInvoiceVm model)
        {
            var invoicePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "invoice.html");
            string mainInvoice = String.Empty;
            using(StreamReader reader = new StreamReader(invoicePath))
            {
                mainInvoice = await reader.ReadToEndAsync();
            }

            string invoiceItemsHtml = String.Empty;
            foreach (var invoiceItem in model.Invoices)
            {
                var invoiceItemPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "invoice-product.html");
                string tempInvoice = String.Empty;
                using (StreamReader reader = new StreamReader(invoiceItemPath))
                {
                    tempInvoice = await reader.ReadToEndAsync();
                }

                tempInvoice= tempInvoice
                            .Replace("{Name}", invoiceItem.Name)
                            .Replace("{Description}", invoiceItem.Description)
                            .Replace("{Count}", invoiceItem.Count.ToString("#.##"))
                            .Replace("{Price}", invoiceItem.Price.ToString("#.##"))
                            .Replace("{Image}", invoiceItem.ImageUrl);
                invoiceItemsHtml += tempInvoice;
            }
            mainInvoice = mainInvoice.Replace("{Products}", invoiceItemsHtml);
            await SendEmailAsync(model.ToEmail, "Invoice", mainInvoice);
        }
    }

    public class EmailServiceOption
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSSL { get; set; }
    }
}
