using ILoveBaku.Application.CQRS.Carts.Models;
using ILoveBaku.Application.CQRS.Payment.Models;
using ILoveBaku.Application.CQRS.User.Queries.GetUser;
using ILoveBaku.Application.CQRS.User.Queries.GetUserAddresses;
using ILoveBaku.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.ViewModels
{
    public class CheckoutVM
    {
        public CheckoutVM()
        {
            Carts = new List<CartDetailDto>();
        }

        public List<CartDetailDto> Carts { get; set; }
        public List<UserAddressInfoDto> Addresses { get; set; }
        public UserDto UserInfo { get; set; }
        public List<PaymentTypeDto> PaymentTypes { get; set; }
        public List<Countries> Countries { get; set; }
    }
}
