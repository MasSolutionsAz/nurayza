using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ILoveBaku.Application.CQRS.Product.Commands.AddProductFile
{
    public class ProductFileDto:IMapFrom<ProductsFiles>
    {
        public bool IsMain { get; set; }
        public string Path { get; set; }

        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductsFiles, ProductFileDto>()
                            .ForMember(c => c.Path, a => a.MapFrom(b => b.Files.Path))
                            .ForMember(c => c.Name, a => a.MapFrom(b => b.Files.Name))
                            .ForMember(c => c.IsMain, a => a.MapFrom(b => b.IsMain));
        }
    }
}