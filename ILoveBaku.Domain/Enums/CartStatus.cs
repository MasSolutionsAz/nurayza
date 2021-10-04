using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Enums
{
    public enum CartStatus : byte
    {
        OnHold = 10,
        Paid = 20,
        OnPayment=30
    }
}
