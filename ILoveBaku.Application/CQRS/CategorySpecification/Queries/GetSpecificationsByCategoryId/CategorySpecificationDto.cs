using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class CategorySpecificationDto:SpecificationDto
    {
        public CategorySpecificationTypeDto Type { get; set; }
        public int Id { get; set; }
        public int Priority { get; set; }

    }
}
