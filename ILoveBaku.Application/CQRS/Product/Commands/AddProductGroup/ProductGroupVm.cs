using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductGroup
{
    public class ProductGroupVm
    {
        [Required(ErrorMessage = "Ad boş qala bilməz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Kateqoriya seçilməyib")]
        public int CategoryId { get; set; }
    }
}
