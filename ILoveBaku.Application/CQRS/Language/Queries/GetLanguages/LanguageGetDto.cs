using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.Language.Queries.GetLanguages
{
    public class LanguageGetDto : IMapFrom<Langs>
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Culture { get; set; }
        public string File { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Langs, LanguageGetDto>()
                .ForMember(d => d.File, opt => opt.MapFrom(s => s.Files.Name));
        }
    }
}
