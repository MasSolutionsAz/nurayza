using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryList
{
    public class CategoryFullDto:IMapFrom<CategoriesLangs>
    {
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public byte LangsId { get; set; }
        public byte Priority { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoriesLangs, CategoryFullDto>()
                            .ForMember(c => c.ParentId, a => a.MapFrom(b => b.Category.ParentId))
                            .ForMember(c => c.Priority, a => a.MapFrom(b => b.Category.Priority))
                            .ForMember(c => c.ParentId, a => a.MapFrom(b => b.Category.ParentId))
                            .ForMember(c => c.IsActive, a => a.MapFrom(b => b.Category.IsActive));
        }
    }
}
