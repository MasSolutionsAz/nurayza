using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ILoveBaku.Application.Common.Extension;

namespace ILoveBaku.Application.CQRS.CategorySpecification.Queries.GetSpecificationsByCategoryId
{
    public class GetSpecificationsByCategoryIdQuery : BaseRequest<ApiResult<List<CategorySpecificationGroupDto>>>
    {
        public int CategoryId { get; set; }
        public class GetSpecificationsByCategoryIdQueryHandler : IRequestHandler<GetSpecificationsByCategoryIdQuery, ApiResult<List<CategorySpecificationGroupDto>>>
        {
            private readonly IApplicationDbContext _context;
            public GetSpecificationsByCategoryIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<ApiResult<List<CategorySpecificationGroupDto>>> Handle(GetSpecificationsByCategoryIdQuery request, CancellationToken cancellationToken)
            {

                #region Query
                var Groups = (from CSG in _context.CategoriesSpecificationsGroupsLangs
                              where CSG.Lang.Culture == request.Culture
                              select new CategorySpecificationGroupDto
                              {
                                  CategorySpecifications = (from CS in _context.CategoriesSpecificationsLangs
                                                            where
                                                            CS.Lang.Culture == request.Culture &&
                                                            CS.CategorySpecification.CategoriesSpecificationGroupId == CSG.CategoriesSpecificationsGroupsId

                                                            join CSR in _context.CategoriesSpecificationsRelations
                                                            on CS.CategoriesSpecificationsId equals CSR.CategoriesSpecificationId
                                                            where CSR.CategoriesId == request.CategoryId

                                                            join CST in _context.CategoriesSpecificationsTypes
                                                            on CS.CategorySpecification.CategoriesSpecificationsTypeId equals CST.Id

                                                            select new CategorySpecificationDto
                                                            {
                                                                Priority = CS.CategorySpecification.Priority,
                                                                Id = CS.CategoriesSpecificationsId,
                                                                Name = CS.Name,
                                                                Properties = (from CSP in _context.CategoriesSpecificationsPropertiesLangs
                                                                              where CSP.Lang.Culture == request.Culture &&
                                                                                    CSP.CategorySpecificationProperty.CategoriesSpecificationId == CS.CategoriesSpecificationsId

                                                                              select new PropertyDto
                                                                              {
                                                                                  Id = CSP.CategoriesSpecificationsPropertiesId,
                                                                                  Name = CSP.Name,
                                                                                  ParentId = (int)CSP.CategorySpecificationProperty.ParentId
                                                                              }).ToList(),

                                                                Type = new CategorySpecificationTypeDto
                                                                {
                                                                    Id = CST.Id,
                                                                    Name = CST.Name,
                                                                    TableName = CST.TableName,
                                                                    Controller = new CategorySpecificationTypeControllerDto
                                                                    {
                                                                        Name = CST.CategoriesSpecificationsTypesController.Name,
                                                                        Specifications = (from CSTCSR in _context.CategorySpecificationsTypesSpecificationsRelations
                                                                                          where CSTCSR.CategorySpecificationsTypesControllersId == CST.CategorySpecificationsTypesControllersId



                                                                                          select new CategorySpecificationTypeControllerSpecificationDto
                                                                                          {
                                                                                              Name = CSTCSR.CategorySpecificationsTypesControllersSpecification.Name,
                                                                                              TableName = CSTCSR.CategorySpecificationsTypesControllersSpecification.TableName,
                                                                                              Values = GetSpecificationValues(CSTCSR.CategorySpecificationsTypesControllersSpecification.TableName, CS.CategoriesSpecificationsId, CSTCSR.CategorySpecificationsTypesControllersSpecificationsId, _context)
                                                                                          }).ToList()
                                                                    }
                                                                }

                                                            }).OrderBy(c=>c.Priority).ToList(),
                                  Name = CSG.Name,
                                  Id = CSG.CategoriesSpecificationsGroupsId

                              }).ToList();
                #endregion
                return ApiResult<List<CategorySpecificationGroupDto>>.CreateResponse(Groups);

            }

            public static List<SpecificationValueDto> GetSpecificationValues(string entity, int categorySpecificationId, int categorySpecificationTypeSpecificationId, IApplicationDbContext _context)
            {

                var data = (from V in _context.GetTable(entity).ToList()
                            join P in _context.CategorySpecificationsTypesControllersSpecificationsProperties
                            on (int)V.GetType().GetProperty("CategorySpecificationsTypesControllersSpecificationsPropertiesId").GetValue(V) equals P.Id
                            where P.CategorySpecificationsTypesControllersSpecificationsId == categorySpecificationTypeSpecificationId

                            where (int)V.GetType().GetProperty("CategorySpecificationsId").GetValue(V) == categorySpecificationId
                            select new SpecificationValueDto
                            {
                                Value = V.GetType().GetProperty("Value") != null ? V.GetType().GetProperty("Value").GetValue(V).ToString() : P.Name
                            }).ToList();

                return data;
            }
        }
    }
}
