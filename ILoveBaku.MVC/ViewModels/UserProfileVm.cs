using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.ViewModels
{
    public class UserProfileVm
    {
        public UserDto UserDto { get; set; }
        public List<ProductCashOutDto> ProductCashOutDto { get; set; }
        public List<UserAddressInfoDto> Addresses { get; set; }
        public List<Countries> Countries { get; set; }
    }
}
