using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CartOrders.Models
{
    public class CartOrderDto
    {
        public int CartId { get; set; }
        public string OrderId { get; set; }
        public string SessionId { get; set; }
    }
}
