using ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.CategorySpecification
{
    public interface ICategorySpecificationService
    {
        Task<object> GetFullDto(int id);
    }
}
