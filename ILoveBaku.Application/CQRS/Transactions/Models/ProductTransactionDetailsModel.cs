using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Transactions.Models
{
    public  class ProductTransactionDetailsModel
    {
        [Required(ErrorMessage ="Barkod qeyd olunmayıb və ya standartlara uyğun deyil.")]
        public string Barcode { get; set; }
        [Required(ErrorMessage = "Maya dəyəri qeyd olunmayıb.")]
        public decimal CostAmount { get; set; }
        [Required(ErrorMessage = "Alış qiyməti qeyd olunmayıb.")]
        public decimal BuyAmount { get; set; }
        [Required(ErrorMessage = "Say qeyd olunmayıb.")]
        public int Count { get; set; }
        public decimal Discount { get; set; }
    }
}
