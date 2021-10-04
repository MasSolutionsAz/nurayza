using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProduct
{
    public class ProductVm
    {
        
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Məhsulun satış qiyməti qeyd olunmalıdır.")]
        public decimal? DefaultSaleAmount { get; set; }
        [Required(ErrorMessage = "Məhsulun alış qiyməti qeyd olunmalıdır.")]
        public decimal? DefaultBuyAmount { get; set; }
        [Required(ErrorMessage = "Məhsulun maya dəyəri qeyd olunmalıdır.")]
        public decimal? DefaultCostAmount { get; set; }
        [Required(ErrorMessage = "Məhsulun barkodu qeyd olunmalıdır.")]
        public string Barcode { get; set; }
        [Required(ErrorMessage = "Məhsulun qrupu qeyd olunmalıdır.")]
        public int? ProductGroupId { get; set; }
        [Required(ErrorMessage ="Satışa buraxılma tarixini qeyd edin.")]
        public DateTime? DefaultPublishDate { get; set; }
        public string DefaultPublishDateDay { get; set; }
        public string DefaultPublishDateMonth { get; set; }
        public string DefaultPublishDateYear { get; set; }
        public string Description { get; set; }
        public List<ProductSpecificationValueDto> Values { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
    }
}
