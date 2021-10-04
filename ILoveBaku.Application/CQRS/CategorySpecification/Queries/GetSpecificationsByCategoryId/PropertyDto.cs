using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }

        public int ProductId { get; set; }
    }
}
