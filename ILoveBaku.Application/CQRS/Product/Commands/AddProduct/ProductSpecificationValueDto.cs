using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProduct
{
    public class ProductSpecificationValueDto
    {
        //[Required]
        public int PropertyId { get; set; }
        //[Required]
        public string TableName { get; set; }
        //[Required]
        public bool MultiData { get; set; }
        public string Value { get; set; }
        public bool IsManual { get; set; }
        public string Type { get; set; }

        public int SpecificationId { get; set; }
        public string SpecificationName { get; set; }
        public string OldValue { get; set; }

        public int ProductId { get; set; }
        public bool ReCreate { get; set; }
    }
}
