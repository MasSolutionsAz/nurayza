using ILoveBaku.Application.CQRS.ProductCashOut.Models;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPacket;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Order
{
    public interface IOrderService
    {
        Task<List<ProductCashOutDto>> GetOrders(int? branchId);
        Task<List<ProductCashOutDetailDto>> GetProductCashOutDetails(int? productCashOutId);
        Task<ProductCashOutDetailVm> CreateVm(int? productCashOutId);
        Task<object> CreatePacket(string name);
        Task<object> CreatePacketDetail(int? productCashOutId,int? packetId);
        Task<ProductCashOutCardDto> GetProductCashOutUserCardDto(int? productCashOutId);
        Task<List<ProductCashOutShippingPacketDto>> GetPackets();
        Task<ProductsCashOutShippingsPacketsDetailsDto> GetProductCashOutShippingPacketByCashOutId(int? productCashOutId);
    }
}
