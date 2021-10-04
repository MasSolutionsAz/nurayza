using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ILoveBaku.Application.CQRS.Product.Queries.GetProductGroups
{
    public class ProductGroupDto:IMapFrom<ProductGroups>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? CategoriesId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
