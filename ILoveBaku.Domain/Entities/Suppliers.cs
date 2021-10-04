using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Domain.Entities
{
    public class Suppliers
    {
        public int Id { get; set; }
        public virtual CompanyDetails CompanyDetails { get; set; }
        public int CompanyDetailsId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedIP { get; set; }
    }
}
