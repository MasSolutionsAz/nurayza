using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class CategorySpecificationTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public CategorySpecificationTypeControllerDto Controller { get; set; }
    }
}
