using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.ProductCashOut.Models;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOuts;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPacket;
using ILoveBaku.Application.CQRS.ProductCashOutShippings.Queries.GetProductCashOutShippingPackets;
using ILoveBaku.MVC.Areas.Admin.Logics.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Areas.Admin.Logics.Order
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        public async Task<ProductCashOutDetailVm> CreateVm(int? productCashOutId)
        {
            var productCashOutDetails = await this.GetProductCashOutDetails(productCashOutId);
            var userCardDto = await this.GetProductCashOutUserCardDto(productCashOutId);
            var packets = await this.GetPackets();
            var shippingPacket = await this.GetProductCashOutShippingPacketByCashOutId(productCashOutId);
            if (productCashOutDetails==null || productCashOutDetails.Count == 0 || userCardDto == null || packets == null)
                return null;

            ProductCashOutDetailVm vm = new ProductCashOutDetailVm
            {
                ProductCashOutDetailDtos = productCashOutDetails,
                UserCardDto = userCardDto,
                Packets = packets,
                PacketDetail = shippingPacket

            };

            return vm;
        }

        public async Task<List<ProductCashOutDto>> GetOrders(int? branchId)
        {
            var data = await API.GetAsync<ApiResult<List<ProductCashOutDto>>>($"products/{branchId}/cashOuts");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<List<ProductCashOutDetailDto>> GetProductCashOutDetails(int? productCashOutId)
        {
            var data = await API.GetAsync<ApiResult<List<ProductCashOutDetailDto>>>($"products/cashOuts/{productCashOutId}/details");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<ProductCashOutCardDto> GetProductCashOutUserCardDto(int? productCashOutId)
        {
            var data = await API.GetAsync<ApiResult<ProductCashOutCardDto>>($"products/cashOuts/{productCashOutId}/card");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<List<ProductCashOutShippingPacketDto>> GetPackets()
        {
            var data = await API.GetAsync<ApiResult<List<ProductCashOutShippingPacketDto>>>("shippings");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<ProductsCashOutShippingsPacketsDetailsDto> GetProductCashOutShippingPacketByCashOutId(int? productCashOutId)
        {
            var data = await API.GetAsync<ApiResult<ProductsCashOutShippingsPacketsDetailsDto>>($"shippings/{productCashOutId}");
            if (data != null && data.Succeeded)
                return data.Response;

            return null;
        }

        public async Task<object> CreatePacket(string name)
        {
            if (name == null || name == "")
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Ad boş qala bilməz." } } };


            var data = await API.PostAsync<string, ApiResult<int?>>($"shippings/packets/?Name={name}", name);

            if (data != null && !data.Succeeded)
                return new { status = 400, errors = data.ErrorList };

            if (data == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Gözlənilməz xəta baş verdi." } } };

            return new { status = 200, data = data.Response };
        }

        public async Task<object> CreatePacketDetail(int? productCashOutId, int? packetId)
        {
            if (productCashOutId == null || packetId == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Gözlənilməz xəta baş verdi." } } };


           
            var data = await API.PostAsync<int?, ApiResult<int?>>($"shippings/packets/{productCashOutId}/details/?packetId={packetId}", packetId);
            if (data != null && !data.Succeeded)
                return new { status = 400, errors = data.ErrorList };

            if (data == null)
                return new { status = 400, errors = new Dictionary<string, string> { { "xeta", "Gözlənilməz xəta baş verdi." } } };

            return new { status = 200, data = data.Response };
        }
    }
}
