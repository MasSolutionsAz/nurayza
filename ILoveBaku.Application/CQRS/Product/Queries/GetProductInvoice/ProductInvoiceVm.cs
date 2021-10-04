using System.Collections.Generic;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductInvoice
{
    public class ProductInvoiceVm
    {
        public List<ProductInvoiceDto> Invoices { get; set; }
        public string ToEmail { get; set; }
    }
}