using AutoMapper;
using ILoveBaku.Application.Common.Mapper;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts
{
    public class ProductCashOutDto:IMapFrom<ProductsCashOut>
    {
        public int CashDeskSeanceId { get; set; }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public List<ProductCashOutDetailDto> Details { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PaymentType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductsCashOut, ProductCashOutDto>()
                            .ForMember(c => c.CashDeskSeanceId, a => a.MapFrom(b => b.CashDeskSeanceId))
                            .ForMember(c => c.Id, a => a.MapFrom(b => b.Id))
                            .ForMember(c => c.CreatedDate, a => a.MapFrom(b => b.CreatedDate))
                            .ForMember(c => c.Status, a => a.MapFrom(b => b.ProductsCashOutStatuses.Name))
                            .ForMember(c => c.Details, a => a.MapFrom(b => b.ProductsCashOutDetails))
                            .ForMember(c => c.Name, a => a.MapFrom(b => b.ProductsCashOutCards.UsersCards.Users.Name))
                            .ForMember(c => c.Surname, a => a.MapFrom(b => b.ProductsCashOutCards.UsersCards.Users.Surname));
        }
    }
}
