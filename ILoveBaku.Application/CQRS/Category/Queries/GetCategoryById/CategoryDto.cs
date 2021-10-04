using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Category.Queries.GetCategoryById
{
    public class CategoryDto : IMapFrom<Categories>
    {
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public byte Priority { get; set; }
        public string Title { get; set; }
    }
}
