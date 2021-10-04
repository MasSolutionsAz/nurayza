using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CartOrders.Models
{
    public static class CartOrderStatusKapitalOrderStatus
    {
        public static readonly Dictionary<string, string> Equivalents = new Dictionary<string, string>()
        {
            { "Gözləmədə", "CREATED" },
            { "Ödənildi", "APPROVED" },
            { "Ləğv edildi", "CANCELED" },
        };
    }
}
