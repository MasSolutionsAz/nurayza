using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class ProductsCashOutAddresses
    {
        public int Id { get; set; }
        public int ProductsCashOutId { get; set; }
        public virtual UsersAddressInfo UsersAddressInfo { get; set; }
        public int UsersAddressInfoId { get; set; }
    }
}
