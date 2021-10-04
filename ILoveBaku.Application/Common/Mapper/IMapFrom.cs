using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.Common.Mapper
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
            profile.CreateMap(GetType(), typeof(T));
        }
    }
}
