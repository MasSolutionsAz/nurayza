using System;
using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.Carts.Models
{
    public class AddCartDetailVM
    {
        public int ProductId { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Məhsulun sayını düzgün daxil edin.")]
        public int Count { get; set; }
    }
}
