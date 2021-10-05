using ILoveBaku.Application.CQRS.Product.Queries.GetProductInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to,string subject,string message);
        Task SendInvoiceAsync(ProductInvoiceVm model);
    }
}
