using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Linq;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails
{
    public class ProductCashOutDetailDto
    {
        //public int Id { get; set; }
        public int ProductsId { get; set; }
        public string ProductName { get; set; }
        public int? ProductsTransactionCount { get; set; }
        public decimal Count { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal TaxPercent { get; set; }
        public string Image { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal PayAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Barcode { get; set; }
        public byte ProductCashOutStatusId { get; set; }
        public int ProductCashOutId { get; set; }



    }
}