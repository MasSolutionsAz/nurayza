using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Models
{
    public class ProductCashOutCardDto:IMapFrom<ProductsCashOutCards>
    {
        public Guid UsersId { get; set; }
        public string  Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public string Phone { get; set; }
        public int PaymentType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductsCashOutCards, ProductCashOutCardDto>()
                            .ForMember(c => c.Name, a => a.MapFrom(b => b.UsersCards.Users.Name))
                            .ForMember(c => c.Surname, a => a.MapFrom(b => b.UsersCards.Users.Surname))
                            .ForMember(c => c.RegisterDate, a => a.MapFrom(b => b.UsersCards.Users.CreatedDate))
                            .ForMember(c => c.FatherName, a => a.MapFrom(b => b.UsersCards.Users.Patronymic))
                            .ForMember(c => c.UsersId, a => a.MapFrom(b => b.UsersCards.UsersId))
                            .ForMember(c => c.BranchName, a => a.MapFrom(b => b.UsersCards.Users.Branches.CompanyDetails.Name))
                            .ForMember(c => c.Phone, a => a.MapFrom(b => b.UsersCards.Users.Phone));
        }
    }
}
