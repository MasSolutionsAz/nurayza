using System;
using System.ComponentModel.DataAnnotations;

namespace ILoveBaku.Application.CQRS.Carts.Models
{
    public class UpdateCartDetailVM
    {
        public int CartDetailId { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Məhsulun sayını düzgün daxil edin.")]
        public int Count { get; set; }
    }
}
